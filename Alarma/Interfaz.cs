using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Alarma
{
    public partial class Form1 : Form
    {
        private DateTime horaAlarma;
        private bool alarmaActivada = false;
        private bool alarmaSonando = false;
        private Timer timerReloj;
        private Timer timerParpadeo;
        private Timer timerMantenerFrente;
        private SoundPlayer player;
        private DateTimePicker dtpHoraAlarma;
        private Label LblEstadoAlarma;  // CREADO DESDE CÓDIGO

        public Form1()
        {
            InitializeComponent();
            ConfigurarControles();
        }

        private void ConfigurarControles()
        {
            // ========== CREAR LABEL ESTADO DESDE CÓDIGO ==========
            LblEstadoAlarma = new Label();
            LblEstadoAlarma.Name = "LblEstadoAlarma";
            LblEstadoAlarma.Text = "ALARMA: DESACTIVADA";
            LblEstadoAlarma.ForeColor = Color.Gray;
            LblEstadoAlarma.BackColor = Color.Transparent;
            LblEstadoAlarma.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            LblEstadoAlarma.Size = new Size(300, 40);
            LblEstadoAlarma.Location = new Point(250, 250);
            LblEstadoAlarma.TextAlign = ContentAlignment.MiddleCenter;
            LblEstadoAlarma.BringToFront();
            this.Controls.Add(LblEstadoAlarma);

            // ========== CONFIGURAR PANEL ROJO ==========
            if (panel1 != null)
            {
                panel1.Dock = DockStyle.Fill;
                panel1.BackColor = Color.Red;
                panel1.Visible = false;
                panel1.SendToBack();
            }

            // ========== CONFIGURAR BOTONES ==========
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Text = "ACTIVAR";

            // ========== DATETIMEPICKER ==========
            dtpHoraAlarma = new DateTimePicker();
            dtpHoraAlarma.Format = DateTimePickerFormat.Time;
            dtpHoraAlarma.ShowUpDown = true;
            dtpHoraAlarma.Location = new Point(350, 300);
            dtpHoraAlarma.Size = new Size(120, 30);
            dtpHoraAlarma.Visible = true;
            dtpHoraAlarma.BringToFront();
            this.Controls.Add(dtpHoraAlarma);

            // ========== TIMERS ==========
            timerReloj = new Timer();
            timerReloj.Interval = 1000;
            timerReloj.Tick += TimerReloj_Tick;
            timerReloj.Start();

            timerParpadeo = new Timer();
            timerParpadeo.Interval = 500;
            timerParpadeo.Tick += TimerParpadeo_Tick;

            timerMantenerFrente = new Timer();
            timerMantenerFrente.Interval = 100;
            timerMantenerFrente.Tick += (s, e) =>
            {
                if (LblEstadoAlarma != null && alarmaSonando)
                    LblEstadoAlarma.BringToFront();
            };

            // ========== EVENTOS ==========
            button1.Click += BtnApagar_Click;
            button2.Click += BtnSnooze_Click;
            button3.Click += BtnActivar_Click;
        }

        private void TimerReloj_Tick(object sender, EventArgs e)
        {
            DateTime ahora = DateTime.Now;
            label2.Text = ahora.ToString("HH:mm:ss");
            label1.Text = ahora.ToString("dddd, dd 'de' MMMM 'de' yyyy");
            VerificarAlarma();
        }

        private void VerificarAlarma()
        {
            if (!alarmaActivada || alarmaSonando) return;
            DateTime ahora = DateTime.Now;
            if (ahora.Hour == horaAlarma.Hour && ahora.Minute == horaAlarma.Minute)
            {
                ActivarAlarma();
            }
        }

        private void ActivarAlarma()
        {
            alarmaSonando = true;

            // Sonido
            try
            {
                if (Properties.Resources.alarma != null)
                {
                    player = new SoundPlayer(Properties.Resources.alarma);
                    player.PlayLooping();
                }
            }
            catch (Exception) { }

            // Panel rojo
            if (panel1 != null)
            {
                panel1.Visible = true;
                panel1.BringToFront();
                timerParpadeo.Start();
            }

            // Label estado
            if (LblEstadoAlarma != null)
            {
                LblEstadoAlarma.Text = "¡¡¡ ALARMA SONANDO !!!";
                LblEstadoAlarma.ForeColor = Color.Navy;
                LblEstadoAlarma.Font = new Font("Segoe UI", 16, FontStyle.Bold);
                LblEstadoAlarma.BackColor = Color.Transparent;
                LblEstadoAlarma.BringToFront();
            }

            timerMantenerFrente.Start();
            this.BackColor = Color.DarkRed;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = false;
        }

        private void TimerParpadeo_Tick(object sender, EventArgs e)
        {
            if (panel1 != null)
            {
                panel1.Visible = !panel1.Visible;
            }

            if (LblEstadoAlarma != null && alarmaSonando)
            {
                if (LblEstadoAlarma.ForeColor == Color.Navy)
                    LblEstadoAlarma.ForeColor = Color.Yellow;
                else
                    LblEstadoAlarma.ForeColor = Color.Navy;
            }
        }

        private void BtnActivar_Click(object sender, EventArgs e)
        {
            if (!alarmaActivada)
            {
                horaAlarma = dtpHoraAlarma.Value;
                alarmaActivada = true;
                button3.Text = "DESACTIVAR";
                LblEstadoAlarma.Text = $"ALARMA: ACTIVADA - {horaAlarma:HH:mm}";
                LblEstadoAlarma.ForeColor = Color.Green;
                LblEstadoAlarma.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                MessageBox.Show($"Alarma activada para las {horaAlarma:HH:mm}", "Alarma");
            }
            else
            {
                alarmaActivada = false;
                button3.Text = "ACTIVAR";
                LblEstadoAlarma.Text = "ALARMA: DESACTIVADA";
                LblEstadoAlarma.ForeColor = Color.Gray;
                MessageBox.Show("Alarma desactivada", "Alarma");
            }
        }

        private void BtnApagar_Click(object sender, EventArgs e)
        {
            DetenerAlarma();
            alarmaActivada = false;
            button3.Text = "ACTIVAR";
            button3.Enabled = true;
            LblEstadoAlarma.Text = "ALARMA: DESACTIVADA";
            LblEstadoAlarma.ForeColor = Color.Gray;
            LblEstadoAlarma.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            this.BackColor = Color.FromArgb(30, 30, 30);
        }

        private void BtnSnooze_Click(object sender, EventArgs e)
        {
            DetenerAlarma();
            horaAlarma = DateTime.Now.AddMinutes(5);
            alarmaActivada = true;
            LblEstadoAlarma.Text = $"ALARMA: POSPUESTA - {horaAlarma:HH:mm}";
            LblEstadoAlarma.ForeColor = Color.Orange;
            button3.Text = "DESACTIVAR";
            button3.Enabled = true;
            this.BackColor = Color.FromArgb(30, 30, 30);
            MessageBox.Show($"Alarma pospuesta 5 minutos. Sonará a las {horaAlarma:HH:mm}", "Snooze");
        }

        private void DetenerAlarma()
        {
            alarmaSonando = false;
            if (player != null)
            {
                player.Stop();
                player.Dispose();
                player = null;
            }
            timerParpadeo.Stop();
            timerMantenerFrente.Stop();
            if (panel1 != null)
                panel1.Visible = false;
            button1.Enabled = false;
            button2.Enabled = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (player != null)
            {
                player.Stop();
                player.Dispose();
            }
            timerReloj?.Stop();
            timerParpadeo?.Stop();
            timerMantenerFrente?.Stop();
            base.OnFormClosing(e);
        }
    }
}