﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace MusicalInstrument
{
    public partial class Form1 : Form
    {
        SignalGenerator sine = new SignalGenerator()
        {
            Type = SignalGeneratorType.Sin,
            Gain = 0.2
        };

        WaveOutEvent player = new WaveOutEvent();

        public Form1()
        {
            InitializeComponent();

            player.Init(sine);

            trackFrequency.ValueChanged += (s, e) => sine.Frequency = trackFrequency.Value;
            trackFrequency.Value = 600;

            trackVolume.ValueChanged += (s, e) => player.Volume = trackVolume.Value / 100F;
            trackVolume.Value = 50;

            panel.MouseDown += (s, e) => player.Play();
            panel.MouseUp += (s, e) => player.Stop();

            trackFrequency.MouseDown += (s, e) => player.Play();
            trackFrequency.MouseUp += (s, e) => player.Stop();

            trackVolume.MouseDown += (s, e) => player.Play();
            trackVolume.MouseUp += (s, e) => player.Stop();
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private System.Drawing.Point CursorPositionOnMouseDown;
        private bool ButtonIsDown = false;

        private void TheMouseDown(object sender, MouseEventArgs e)
        {
            player.Play();
            CursorPositionOnMouseDown = e.Location;
            ButtonIsDown = true;
        }

        private void TheMouseUp(object sender, MouseEventArgs e)
        {
            player.Stop();
            ButtonIsDown = false;
        }

        private void panel_MouseMove(object sender, MouseEventArgs e)
        {
            var dX = e.X - CursorPositionOnMouseDown.X;
            var vol = player.Volume + (dX / 1000F);

            var dY = CursorPositionOnMouseDown.Y - e.Y;
            var freq = sine.Frequency + dY;

            if (ButtonIsDown)
            {
                player.Volume = (vol > 0) ? (vol < 1) ? vol : 1: 0;

                sine.Frequency = (freq > 100) ? (freq < 1000) ? freq : 1000 : 100;

                trackFrequency.Value = (int)Math.Round(sine.Frequency);
                trackVolume.Value = (int)Math.Round(player.Volume * 100);
            }

            Text = $"Musical Instrument! ({dX}, {dY}) ({vol}, {freq})";
        }
    }
}
