# ParticleHeartAnimation — WPF Particle Heart Animation with Interactive Pulse Effect

ParticleHeartAnimation is a visually appealing WPF desktop application that draws a heart shape using thousands of animated particles. It features a heartbeat pulsing effect and interactive particle repulsion on mouse movement.

---

## Features

- Draws a heart shape formed by 3700 animated particles.
- Smooth particle movement towards their target heart shape positions.
- Heartbeat pulsing effect with smooth scaling animation.
- Interactive repulsion of particles when the mouse cursor approaches.
- Clean and responsive UI built with WPF.

---

## Technologies

- C# (.NET 6/7)
- WPF (Windows Presentation Foundation)
- DispatcherTimer and CompositionTarget.Rendering for smooth animation
- Storyboard and DoubleAnimation for heartbeat pulsing

---

## How It Works

1. On startup, particles are randomly placed on the canvas and then smoothly move to form a heart shape.
2. A heartbeat timer periodically triggers a scale animation, making the heart pulse rhythmically.
3. Moving the mouse near the heart repels nearby particles dynamically, creating an interactive effect.
4. The animation runs continuously, updating particle positions each frame for smooth motion.

---

## Getting Started

To run the application locally, follow these steps:

```bash
git clone https://github.com/yourusername/ParticleHeartAnimation.git
