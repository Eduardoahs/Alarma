using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloReFun
{
    internal class Memoria
    {
        public TimeSpan nuevaHora;
        public bool alarmaActiva = false;
        public bool sonidoAlarma = false;

        private static Memoria singleton;

        private Memoria(){
        }
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
