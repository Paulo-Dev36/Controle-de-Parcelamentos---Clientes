using Org.BouncyCastle.Bcpg.OpenPgp;
using PARCELAMENTOS_EMPRESA.Classes;
using PARCELAMENTOS_EMPRESA.Repositorios;
using PARCELAMENTOS_EMPRESA.Validadores;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PARCELAMENTOS_EMPRESA.Formularios
{
    public partial class FrmEditarParcelamento : Form
    {
        private RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
        private RepositorioParcelamento repositorioParcelamento = new RepositorioParcelamento();
        private ValidaParcelamento validaParcelamento = new ValidaParcelamento();
        private ValidaData validaData = new ValidaData();
        public int IdUsuario { get; set; }
        public int IdParcelamento { get; set; }
        public FrmEditarParcelamento(IEnumerable<ParcelamentosEmpresa> parcelamentosEmpresa) //, string cidade, string atividade, string regime, string data, bool possuiParcelamento,string numeroParcela, string tipo, bool enviado
        {
            InitializeComponent();
            MapeiaNomeRegimes();
            IdParcelamento = parcelamentosEmpresa.First().Id;
            textBoxNomeEmpresa.Text = parcelamentosEmpresa.First().Empresa.First().NomeEmpresa;
            textBoxCidade.Text = parcelamentosEmpresa.First().Cidade;
            textBoxAtividade.Text = parcelamentosEmpresa.First().Atividade;
            comboBoxRegimes.Text = parcelamentosEmpresa.First().Regime;
            maskedTextBoxData.Text = parcelamentosEmpresa.First().Data.ToString();
            IdUsuario = parcelamentosEmpresa.First().Usuario.First().Id;
            string possuiParcel = parcelamentosEmpresa.First().Parcelamento.ToString();

            if (parcelamentosEmpresa.First().Parcelamento.ToString().Equals("SIM"))
            {
                checkBoxSim.Checked = true;
                checkBoxNao.Checked = false;
            }
            else
            {
                checkBoxSim.Checked = false;
                checkBoxNao.Checked = true;
            }

            textBoxParcela.Text = parcelamentosEmpresa.First().Parcela;
            textBoxTipos.Text = parcelamentosEmpresa.First().Tipo;

            if (parcelamentosEmpresa.First().Status.ToString().Equals("ENVIADO"))
            {
                checkBoxEnviado.Checked = true;
                checkBoxNaoEnviado.Checked = false;
            }
            else
            {
                checkBoxEnviado.Checked = false;
                checkBoxNaoEnviado.Checked = true;
            }
        }

        private void MapeiaNomeRegimes()
        {
            comboBoxRegimes.Items.Add("SIMPLES");
            comboBoxRegimes.Items.Add("SEM ENQUADRAMENTO");
            comboBoxRegimes.Items.Add("MEI");
            comboBoxRegimes.Items.Add("LUCRO REAL");
            comboBoxRegimes.Items.Add("LUCRO PRESUMIDO");
            comboBoxRegimes.Items.Add("DOMÉSTICA");
        }

        private void Salvar(object sender, EventArgs e)
        {
            ParcelamentosEmpresa parcelamentosEmpresa = new ParcelamentosEmpresa();
            string parcelamento = checkBoxSim.Checked ? "SIM" : "NAO";

            if (validaParcelamento.EhCampoVazio(textBoxCidade.Text, textBoxAtividade.Text, comboBoxRegimes.Text, parcelamento))
                return;

            if (validaData.EhDataInvalida(maskedTextBoxData.Text))
                return;            

            string status = checkBoxEnviado.Checked ? "ENVIADO" : "NÃO ENVIADO";

            DateTime.TryParse(maskedTextBoxData.Text, out DateTime data);

            parcelamentosEmpresa.Id = IdParcelamento;
            parcelamentosEmpresa.Cidade = textBoxCidade.Text;
            parcelamentosEmpresa.Atividade = textBoxAtividade.Text;
            parcelamentosEmpresa.Regime = comboBoxRegimes.Text;
            parcelamentosEmpresa.Parcelamento = parcelamento;
            parcelamentosEmpresa.Parcela = textBoxParcela.Text;
            parcelamentosEmpresa.Data = data;
            parcelamentosEmpresa.Status = status;
            parcelamentosEmpresa.Tipo = textBoxTipos.Text;
            parcelamentosEmpresa.Usuario = repositorioUsuario.Get(x => x.Id.Equals(IdUsuario));

            repositorioParcelamento.Update(parcelamentosEmpresa);

            MessageBox.Show("Parcelamento modificado com sucesso.", "SUCESSO", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void checkBoxSim_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxNao.Checked = false;
        }

        private void checkBoxNao_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxSim.Checked = false;
        }

        private void checkBoxEnviado_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxNaoEnviado.Checked = false;
        }

        private void checkBoxNaoEnviado_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxEnviado.Checked = false;
        }
    }
}
