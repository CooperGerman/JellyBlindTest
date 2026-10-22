(function () {
  'use strict';

  const LINK_ID = 'jellyblindtest-main-menu-link';
  const OVERLAY_ID = 'jellyblindtest-user-page';
  const STYLE_ID = 'jellyblindtest-user-page-styles';
  let escapeHandler = null;

  function getApi() {
    return window.ApiClient ?? null;
  }

  function installStyles() {
    if (document.getElementById(STYLE_ID)) return;
    const style = document.createElement('style');
    style.id = STYLE_ID;
    style.textContent =
      '#' + OVERLAY_ID + '{position:fixed;inset:0;z-index:100000;display:flex;flex-direction:column;background:#10171b}' +
      '#' + OVERLAY_ID + ' .jbt-user-header{display:flex;min-height:54px;align-items:center;gap:16px;padding:8px 18px;border-bottom:1px solid #35464b;background:#151f23;color:#f4f5f3}' +
      '#' + OVERLAY_ID + ' .jbt-user-title{flex:1;margin:0;font-size:17px;font-weight:650}' +
      '#' + OVERLAY_ID + ' .jbt-user-close{min-height:38px;padding:7px 12px;border:1px solid #52666b;border-radius:4px;color:#f4f5f3;background:#243238;font:inherit;cursor:pointer}' +
      '#' + OVERLAY_ID + ' .jbt-user-frame{width:100%;height:100%;flex:1;border:0;background:#10171b}' +
      '#' + OVERLAY_ID + ' .jbt-user-error{margin:auto;color:#ffd166;font:16px system-ui,sans-serif}';
    document.head.appendChild(style);
  }

  function closeGame() {
    document.getElementById(OVERLAY_ID)?.remove();
    if (escapeHandler) {
      document.removeEventListener('keydown', escapeHandler);
      escapeHandler = null;
    }
  }

  function openGame() {
    if (document.getElementById(OVERLAY_ID)) return;
    const api = getApi();
    if (!api) return;

    installStyles();
    const overlay = document.createElement('div');
    overlay.id = OVERLAY_ID;

    const header = document.createElement('div');
    header.className = 'jbt-user-header';
    const title = document.createElement('h2');
    title.className = 'jbt-user-title';
    title.textContent = 'Blind Test';
    const close = document.createElement('button');
    close.className = 'jbt-user-close';
    close.type = 'button';
    close.textContent = 'Back to Jellyfin';
    close.addEventListener('click', closeGame);
    header.append(title, close);

    const frame = document.createElement('iframe');
    frame.className = 'jbt-user-frame';
    frame.title = 'Blind Test game';
    overlay.append(header, frame);
    document.body.appendChild(overlay);

    escapeHandler = event => {
      if (event.key === 'Escape') closeGame();
    };
    document.addEventListener('keydown', escapeHandler);

    frame.addEventListener('error', () => {
      const error = document.createElement('p');
      error.className = 'jbt-user-error';
      error.textContent = 'Could not open Blind Test. Please try again.';
      overlay.appendChild(error);
    }, { once: true });
    frame.src = api.getUrl('jellyblindtest/page');
  }

  function addMenuLink() {
    if (document.getElementById(LINK_ID)) return;
    const header = document.querySelector('header');
    if (!header) return;

    const anchor = header.querySelector('a[href^="#/home?tab="]');
    const menu = anchor?.parentElement;
    if (!menu) return;

    const template = menu.querySelector('a[href*="collectionType=playlists"]') ?? anchor;
    const link = template.cloneNode(false);
    link.id = LINK_ID;
    link.href = '#';
    const icon = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
    icon.setAttribute('viewBox', '0 0 24 24');
    icon.setAttribute('width', '24');
    icon.setAttribute('height', '24');
    icon.setAttribute('aria-hidden', 'true');
    icon.setAttribute('focusable', 'false');
    icon.style.flex = 'none';
    icon.style.marginRight = '8px';
    icon.innerHTML = '<path d="M7.2 18.6V7.3l6.1-1.2v9.1" fill="none" stroke="currentColor" stroke-width="3.1" stroke-linecap="square" stroke-linejoin="miter"/><ellipse cx="4.1" cy="19" rx="3.4" ry="2.6" transform="rotate(-20 4.1 19)" fill="currentColor"/><ellipse cx="13.1" cy="15.7" rx="3.1" ry="2.4" transform="rotate(-20 13.1 15.7)" fill="currentColor"/><circle cx="18" cy="5" r="4.7" fill="currentColor"/><path d="M16.3 4c.1-1.2.9-2 2-2s2 .8 2 1.9c0 .9-.5 1.5-1.3 2-.7.5-1 1-1 1.8" fill="none" stroke="#10171b" stroke-width="1.25" stroke-linecap="round"/><circle cx="18" cy="9.2" r=".65" fill="#10171b"/>';
    const label = document.createElement('span');
    label.textContent = 'Blind Test';
    link.append(icon, label);
    link.setAttribute('aria-label', 'Open Blind Test');
    link.setAttribute('title', 'Blind Test');
    link.addEventListener('click', event => {
      event.preventDefault();
      openGame();
    });

    const playlistsLink = [...menu.querySelectorAll('a')]
      .find(item => item.textContent.trim() === 'Playlists');
    if (playlistsLink) playlistsLink.insertAdjacentElement('afterend', link);
    else menu.appendChild(link);
  }

  const observer = new MutationObserver(addMenuLink);
  observer.observe(document.documentElement, { childList: true, subtree: true });
  window.setInterval(addMenuLink, 1000);
  addMenuLink();
})();