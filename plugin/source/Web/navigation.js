(function () {
  'use strict';

  const TAB_ID = 'blind-test';
  const TAB_TITLE = 'Blind Test';
  const MAX_REGISTRY_WAIT_MS = 60000;
  const RETRY_INTERVAL_MS = 100;
  const MOUNT_MARKER = 'data-jbt-game-mounted';
  const startedAt = Date.now();

  function getNativeTabs() {
    return window.JellyfinEnhanced?.nativeTabs
      ?? window.parent?.JellyfinEnhanced?.nativeTabs
      ?? null;
  }

  async function mountGame(panel) {
    if (panel.hasAttribute(MOUNT_MARKER)) return;
    panel.setAttribute(MOUNT_MARKER, 'true');
    const api = window.ApiClient;
    if (!api) {
      panel.textContent = 'Jellyfin API unavailable. Reload the page and try again.';
      return;
    }

    try {
      const response = await fetch(api.getUrl('jellyblindtest/page'), { credentials: 'same-origin' });
      if (!response.ok) throw new Error('Could not load the Blind Test page.');

      const pageDocument = new DOMParser().parseFromString(await response.text(), 'text/html');
      const gamePage = pageDocument.getElementById('JellyBlindTestPage');
      if (!gamePage) throw new Error('Blind Test page markup was not found.');
      gamePage.querySelectorAll('script').forEach(script => script.remove());
      panel.replaceChildren(gamePage);

      const gameScript = document.createElement('script');
      gameScript.src = api.getUrl('jellyblindtest/game.js');
      gameScript.onerror = () => {
        panel.textContent = 'Could not load Blind Test. Reload Jellyfin and try again.';
      };
      document.head.appendChild(gameScript);
    } catch (error) {
      panel.textContent = error?.message ?? 'Could not load Blind Test.';
    }
  }

  function registerNativeTab() {
    const registry = getNativeTabs();
    if (registry && typeof registry.register === 'function') {
      registry.register(TAB_ID, TAB_TITLE, mountGame, 'music_note');
      return;
    }

    if (Date.now() - startedAt < MAX_REGISTRY_WAIT_MS) {
      window.setTimeout(registerNativeTab, RETRY_INTERVAL_MS);
      return;
    }

    console.warn('Blind Test Home tab requires Jellyfin Enhanced native-tabs support.');
  }

  registerNativeTab();
})();