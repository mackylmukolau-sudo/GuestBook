'use strict';

document.addEventListener('DOMContentLoaded', () => {

  // ── Patch ASP.NET validation error spans ───────────────────────────────
  document.querySelectorAll('.field-error').forEach(patchError);

  new MutationObserver(mutations => {
    mutations.forEach(m => {
      m.addedNodes.forEach(n => {
        if (n.nodeType === 1 && n.classList?.contains('field-error')) patchError(n);
      });
      if (m.type === 'characterData') {
        const el = m.target.parentElement;
        if (el?.classList?.contains('field-error')) patchError(el);
      }
    });
  }).observe(document.body, { childList: true, subtree: true, characterData: true });

  // ── Real-time input validation ─────────────────────────────────────────
  document.querySelectorAll('.form-input').forEach(input => {
    input.addEventListener('blur', () => {
      const name = input.name || input.id;
      const err = document.querySelector(`[data-valmsg-for="${name}"]`);
      const hasError = err?.classList.contains('field-validation-error');
      if (input.value.trim() && !hasError) input.classList.add('is-valid');
      else input.classList.remove('is-valid');
    });

    input.addEventListener('input', () => input.classList.remove('is-valid'));
  });
});

function patchError(span) {
  if (!span || span.dataset.patched) return;
  const textSpan = span.querySelector('.field-error-text');
  if (!textSpan) return;
  const raw = Array.from(span.childNodes)
    .filter(n => n.nodeType === 3)
    .map(n => n.textContent.trim()).join(' ').trim();
  if (raw) {
    textSpan.textContent = raw;
    Array.from(span.childNodes).filter(n => n.nodeType === 3).forEach(n => n.remove());
  }
  span.dataset.patched = '1';
}
