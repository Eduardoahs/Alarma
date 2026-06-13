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
    
    public partial class Interfaz : Form
    {
    private TimeSpan horaAlarma;
    private bool alarmaSonando = false;


    private Memoria memoria = Memoria.Instancia;
        public Interfaz()
        {
            InitializeComponent();

        }
            public string RevisarEstadoAlarma()
            {
                if (alarmaSonando)
                {
                    return "SONANDO";
                }
                else if (memoria.alarmaActiva)
                {
                    return $"ACTIVADA - Hora: {memoria.nuevaHora:hh\\:mm}";
                }
                else
                {
                    return "DESACTIVADA";
                }
            }

            
            public bool EjecutarSnooze()
            {
                if (!alarmaSonando)
                {
                    return false; 
                }

                
                memoria.sonidoAlarma = false;
                alarmaSonando = false;

                
                memoria.nuevaHora = DateTime.Now.AddMinutes(5).TimeOfDay;
                memoria.alarmaActiva = true;

                return true; 
            }

            
            public bool EjecutarApagar()
            {
                if (!alarmaSonando && !memoria.alarmaActiva)
                {
                    return false; 
                }

                
                memoria.alarmaActiva = false;
                memoria.sonidoAlarma = false;
                alarmaSonando = false;

                return true; 
            }

            
            internal class Memoria
            {
                public TimeSpan nuevaHora;
                public bool alarmaActiva = false;
                public bool sonidoAlarma = false;

                private static Memoria singleton;

                private Memoria() { }

                public static Memoria Instancia
                {
                    get
                    {
                        if (singleton == null)
                        {
                            singleton = new Memoria();
                        }
                        return singleton;
                    }
                }

                public void RevisarAlarma(TimeSpan horaActual)
                {
                    if (alarmaActiva == true)
                    {
                        if (horaActual.Hours == nuevaHora.Hours && horaActual.Minutes == nuevaHora.Minutes)
                        {
                            sonidoAlarma = true;
                            new System.Media.SoundPlayer(Properties.Resources.alarma).PlayLooping();
                        }
                    }
                }
            }
        }
    }