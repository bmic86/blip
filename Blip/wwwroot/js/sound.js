const audioCtx = new (window.AudioContext || window.webkitAudioContext)();
let oscillator = null;

export function play(timeInSeconds) {
  if (oscillator == null) {
    startNewOscillator();
  }
  oscillator.stop(audioCtx.currentTime + timeInSeconds);
}

export function pause() {
  audioCtx.suspend();
}

export function resume() {
  audioCtx.resume();
}

function startNewOscillator() {
  oscillator = audioCtx.createOscillator();
  oscillator.type = "square";
  oscillator.onended = () => {
    oscillator = null;
  };
  oscillator.connect(audioCtx.destination);
  oscillator.frequency.setValueAtTime(400, audioCtx.currentTime) //Hz
  oscillator.start(audioCtx.currentTime);
}
