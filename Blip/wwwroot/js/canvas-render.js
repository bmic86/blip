let _screen = null
let _frameBuffer = null

export function init(canvasId, frameWidth, frameHeight) {
  initScreen(canvasId)
  initFrameBuffer(frameWidth, frameHeight)
}

export function drawPixel(x, y, value) {
  let i = y * 4 * _frameBuffer.image.width + x * 4
  _frameBuffer.image.data[i + 0] = value ? 255 : 0;
  _frameBuffer.image.data[i + 1] = value ? 255 : 0;
  _frameBuffer.image.data[i + 2] = value ? 255 : 0;
  _frameBuffer.image.data[i + 3] = 255;
}

export function clearFrame() {
  for (let i = 0; i < _frameBuffer.image.data.length; i += 4) {
    _frameBuffer.image.data[i + 0] = 0;
    _frameBuffer.image.data[i + 1] = 0;
    _frameBuffer.image.data[i + 2] = 0;
    _frameBuffer.image.data[i + 3] = 255;
  }
}

export function renderFrame() {
  _frameBuffer.context.putImageData(_frameBuffer.image, 0, 0)
  window.requestAnimationFrame(() => {
    _screen.context.drawImage(_frameBuffer.canvas, 0, 0, 64, 32, 0, 0, _screen.canvas.width, _screen.canvas.height)
  })
}


function initFrameBuffer(frameWidth, frameHeight) {
  const bufferCanvas = document.createElement('canvas')
  const bufferContext = bufferCanvas.getContext('2d')
  _frameBuffer = {
    image: bufferContext.createImageData(frameWidth, frameHeight),
    canvas: bufferCanvas,
    context: bufferContext,
  };
}

export function initScreen(canvasId) {
  const screenCanvas = document.getElementById(canvasId)
  _screen = {
    canvas: screenCanvas,
    context: screenCanvas.getContext('2d'),
  };
  _screen.context.imageSmoothingEnabled = false
}