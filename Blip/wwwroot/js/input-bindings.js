let _dotNetHandler = null;

let onKeyUp = (e) => {
  _dotNetHandler.invokeMethod('OnDocumentKeyUp', e.code)
}

let onKeyDown = (e) => {
  _dotNetHandler.invokeMethod('OnDocumentKeyDown', e.code)
}

export function registerInputHandlers(dotNetHandler) {
  if (_dotNetHandler == null) {
    document.addEventListener('keyup', onKeyUp)
    document.addEventListener('keydown', onKeyDown)

    _dotNetHandler = dotNetHandler
  }
}

export function unregisterInputHandlers() {
  if (_dotNetHandler != null) {
    document.removeEventListener('keyup', onKeyUp)
    document.removeEventListener('keydown', onKeyDown)

    _dotNetHandler = null
  }
}