using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Forms;

namespace PARCELAMENTOS_EMPRESA.Validadores
{
    public class ValidaData
    {

        public bool EhDataInvalida(string periodoInicial, string periodoFinal)
        {
            string validaPeriodoInicial = periodoInicial.Replace('/', ' ');
            string validaPeriodoFinal = periodoFinal.Replace('/', ' ');
            string[] val = periodoInicial.Split('/');
            string[] val2 = periodoFinal.Split('/');
            int maiorAnoPermitido = 2050;
            int menorAnoPermitido = 2000;
            int diaInicial, mesInicial, anoInicial, diaFinal, mesFinal, anoFinal;

            if (string.IsNullOrWhiteSpace(validaPeriodoInicial))
            {
                MessageBox.Show("O período inicial esta vazio!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }

            if (string.IsNullOrWhiteSpace(validaPeriodoFinal))
            {
                MessageBox.Show("O período final esta vazio!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }


            if (int.TryParse(val[0], out diaInicial) && int.TryParse(val[1], out mesInicial) && int.TryParse(val[2], out anoInicial))
            {
                if (anoInicial >= menorAnoPermitido && anoInicial <= maiorAnoPermitido)
                {
                    if (mesInicial >= 1 && mesInicial <= 12)
                    {
                        int maxDia = (mesInicial == 2 ? (anoInicial % 4 == 0 ? 29 : 28) : mesInicial <= 7 ? (mesInicial % 2 == 0 ? 30 : 31) : (mesInicial % 2 == 0 ? 31 : 30));

                        if (diaInicial < 1 || diaInicial > maxDia)
                        {
                            MessageBox.Show("Dia inicial inválido!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mês inicial inválido", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("Ano inicial inválido", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                }
            }
            else
            {
                MessageBox.Show("Data inicial inválida", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }

            if (int.TryParse(val2[0], out diaFinal) && int.TryParse(val2[1], out mesFinal) && int.TryParse(val2[2], out anoFinal))
            {
                if (anoFinal >= menorAnoPermitido && anoFinal <= maiorAnoPermitido)
                {
                    if (mesFinal >= 1 && mesFinal <= 12)
                    {
                        int maxDia = (mesFinal == 2 ? (anoFinal % 4 == 0 ? 29 : 28) : mesFinal <= 7 ? (mesFinal % 2 == 0 ? 30 : 31) : (mesFinal % 2 == 0 ? 31 : 30));

                        if (diaFinal < 1 || diaFinal > maxDia)
                        {
                            MessageBox.Show("Dia final inválido!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mês final inválido", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("Ano final inválido", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                }
            }
            else
            {
                MessageBox.Show("Data final inválida", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }

            DateTime periodoInicialData = Convert.ToDateTime(periodoInicial);
            DateTime periodoFinallData = Convert.ToDateTime(periodoFinal);

            if (periodoInicialData.Equals(DateTime.MinValue))
            {
                MessageBox.Show("O período inicial não é válido!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (periodoFinallData.Equals(DateTime.MinValue))
            {
                MessageBox.Show("O período final não é válido!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return false;
        }

        public bool EhDataInvalida(string data)
        {
            string validaPeriodoInicial = data.Replace('/', ' ');
            
            if (string.IsNullOrWhiteSpace(validaPeriodoInicial))
            {
                MessageBox.Show("O período esta vazio!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }

            DateTime periodoData = Convert.ToDateTime(data);
            

            if (periodoData.Equals(DateTime.MinValue))
            {
                MessageBox.Show("O período inicial não é válido!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return false;
        }
    }
}
