using System.Windows.Forms;

namespace PARCELAMENTOS_EMPRESA.Validadores
{
    public class ValidaParcelamento
    {
        public bool EhCampoVazio(string cidade, string atividade, string regime, string parcelamento)
        {
            if (string.IsNullOrWhiteSpace(cidade))
            {
                MessageBox.Show("O campo cidade esta vazio!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }

            if (string.IsNullOrWhiteSpace(atividade))
            {
                MessageBox.Show("O campo atividade esta vazio!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }

            if (string.IsNullOrWhiteSpace(regime))
            {
                MessageBox.Show("O campo regime esta vazio!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }

            if (string.IsNullOrWhiteSpace(parcelamento))
            {
                MessageBox.Show("O campo possui parcelamento esta vazio!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            return false;
        }
    }
}
