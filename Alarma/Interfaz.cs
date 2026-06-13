using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alarma
{
    private TimeSpan horaAlarma;
    private bool alarmaSonando = false;


    private Memoria memoria = Memoria.Instancia;
    public partial class Interfaz : Form
    {
        public Interfaz()
        {
            InitializeComponent();

        }

            private void Form1_Load(object sender, EventArgs e)
            {
                timerHora.Enabled = true;
                timerAlarma.Enabled = true;
            }

            private void btnSnooze_Click(object sender, EventArgs e)
            {
                if (alarmaSonando)
                {
                    DetenerSonido();
                    DetenerAvisoVisual();

                    horaAlarma = DateTime.Now.AddMinutes(5).TimeOfDay;
                    memoria.nuevaHora = horaAlarma;
                    memoria.alarmaActiva = true;
                    alarmaSonando = false;

                    lblEstadoAlarma.Text = $"Alarma pospuesta hasta {horaAlarma:hh\\:mm}";
                    lblEstadoAlarma.ForeColor = System.Drawing.Color.Orange;
                    lblEstadoDetallado.Text = "Alarma: POSPUESTA (SNOOZE) | Próxima alarma en 5 min";
                }
            }

            private void btnApagarAlarma_Click(object sender, EventArgs e)
            {
                if (alarmaSonando)
                {
                    DetenerSonido();
                    DetenerAvisoVisual();

                    memoria.alarmaActiva = false;
                    alarmaSonando = false;

                    lblEstadoAlarma.Text = "Desactivada";
                    lblEstadoAlarma.ForeColor = System.Drawing.Color.Red;
                    lblEstadoDetallado.Text = "Alarma: DESACTIVADA | Estado: INACTIVA";
                    lblCuentaRegresiva.Text = "Alarma apagada";
                }
            }

            private void TimerAlarma_Tick(object sender, EventArgs e)
            {
                DateTime ahora = DateTime.Now;
                TimeSpan horaActual = ahora.TimeOfDay;

                memoria.RevisarAlarma(horaActual);

                if (memoria.sonidoAlarma && !alarmaSonando)
                {
                    alarmaSonando = true;
                    ActivarAvisoVisual();
                }

                if (memoria.alarmaActiva && !alarmaSonando)
                {
                    lblEstadoDetallado.Text = "Alarma: ACTIVADA | Estado: ESPERANDO";
                    lblEstadoDetallado.ForeColor = System.Drawing.Color.Green;

                    TimeSpan tiempoRestante = new DateTime(ahora.Year, ahora.Month, ahora.Day,
                        memoria.nuevaHora.Hours, memoria.nuevaHora.Minutes, 0) - ahora;

                    if (tiempoRestante.TotalSeconds > 0)
                    {
                        lblCuentaRegresiva.Text = $"Faltan: {tiempoRestante.Minutes:D2}:{tiempoRestante.Seconds:D2}";
                    }
                    else if (tiempoRestante.TotalSeconds <= 0 && !alarmaSonando)
                    {
                        lblCuentaRegresiva.Text = "¡Alarma por sonar!";
                    }
                }
                else if (!memoria.alarmaActiva)
                {
                    lblEstadoDetallado.Text = "Alarma: DESACTIVADA | Estado: INACTIVA";
                    lblEstadoDetallado.ForeColor = System.Drawing.Color.Red;
                }
                else if (alarmaSonando)
                {
                    lblEstadoDetallado.Text = "Alarma: ACTIVADA | Estado: SONANDO 🔔";
                    lblEstadoDetallado.ForeColor = System.Drawing.Color.Red;
                }
            }

            private void ActivarAvisoVisual()
            {
                this.BackColor = System.Drawing.Color.Red;
                lblEstadoAlarma.Text = "¡ALARMA SONANDO!";
                lblEstadoAlarma.ForeColor = System.Drawing.Color.Yellow;
                lblEstadoDetallado.Text = "🔔 ¡ALARMA ACTIVADA! 🔔";
            }

            private void DetenerAvisoVisual()
            {
                this.BackColor = System.Drawing.SystemColors.Control;
                if (!memoria.alarmaActiva)
                {
                    lblEstadoAlarma.Text = "Desactivada";
                    lblEstadoAlarma.ForeColor = System.Drawing.Color.Red;
                }
            }

            private void DetenerSonido()
            {
                memoria.sonidoAlarma = false;
            }
        }
    }
}
