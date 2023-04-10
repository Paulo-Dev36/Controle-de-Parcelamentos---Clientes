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

namespace PARCELAMENTOS_EMPRESA
{
    public partial class FrmAdicionarParcelamento : Form
    {
        private readonly RepositorioEmpresa repositorioEmpresa = new RepositorioEmpresa();
        private RepositorioParcelamento repositorioParcelamento = new RepositorioParcelamento();
        private ParcelamentosEmpresa parcelamentosEmpresa = new ParcelamentosEmpresa();
        private ValidaData validaData = new ValidaData();
        private ValidaParcelamento validaParcelamento = new ValidaParcelamento();
        private RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
        public string NomeUsuario { get; set; }
        public FrmAdicionarParcelamento(string nomeUsuario)
        {
            InitializeComponent();
            MapeiaNomeEmpresas();
            MapeiaNomeRegimes();
            NomeUsuario = nomeUsuario;
        }

        private void MapeiaNomeEmpresas()
        {
            foreach (String nomeEmpresas in repositorioEmpresa.ListaNomeEmpresas())
            {
                comboBoxEmpresas.Items.Add(nomeEmpresas);
            }
            comboBoxEmpresas.SelectedIndex = 0;
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
            string[] stringEmpresa = comboBoxEmpresas.Text.Split('-');
            int codigoEmpresa = Convert.ToInt32(stringEmpresa[0]);
            int filialEmpresa = Convert.ToInt32(stringEmpresa[1]);
            
            string parcelamento = checkBoxSim.Checked ? "SIM" : "NAO";

            string status = checkBoxEnviado.Checked ? "ENVIADO" : "NÃO ENVIADO"; 

            if (validaParcelamento.EhCampoVazio(textBoxCidade.Text, textBoxAtividade.Text, comboBoxRegimes.Text, parcelamento))
                return;

            if (validaData.EhDataInvalida(maskedTextBoxData.Text))
                return;

            DateTime.TryParse(maskedTextBoxData.Text, out DateTime data);

            parcelamentosEmpresa.Empresa = repositorioEmpresa.ObterEmpresaPorCodigoEFilial(codigoEmpresa, filialEmpresa);
            parcelamentosEmpresa.Cidade = textBoxCidade.Text;
            parcelamentosEmpresa.Atividade = textBoxAtividade.Text;
            parcelamentosEmpresa.Regime = comboBoxRegimes.Text;
            parcelamentosEmpresa.Parcelamento = parcelamento;
            parcelamentosEmpresa.Tipo = textBoxTipo.Text;
            parcelamentosEmpresa.Parcela = textBoxParcela.Text;
            parcelamentosEmpresa.Data = data;
            parcelamentosEmpresa.Status = status;
            parcelamentosEmpresa.Usuario = repositorioUsuario.Get(x => x.NomeUsuario.Equals(NomeUsuario));
            repositorioParcelamento.Add(parcelamentosEmpresa);

            MessageBox.Show("Parcelamento adicionado com sucesso.", "SUCESSO", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

        private void CheckBoxSim_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxNao.Checked = false;
        }

        private void CheckBoxNao_CheckedChanged(object sender, EventArgs e)
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
