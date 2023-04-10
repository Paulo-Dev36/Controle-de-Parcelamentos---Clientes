using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Office.Interop.Excel;
using PARCELAMENTOS_EMPRESA.Classes;
using PARCELAMENTOS_EMPRESA.Formularios;
using PARCELAMENTOS_EMPRESA.Repositorios;
using PARCELAMENTOS_EMPRESA.Validadores;
using RelatoriosAtendimento;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;
using Excel = Microsoft.Office.Interop.Excel;
using Fonte = iTextSharp.text.Font;

namespace PARCELAMENTOS_EMPRESA
{
    public partial class FrmControleParcelamentos : Form
    {
        private readonly RepositorioEmpresa repositorioEmpresa = new RepositorioEmpresa();
        private readonly RepositorioParcelamento repositorioParcelamento = new RepositorioParcelamento();
        private readonly ValidaData validaData = new ValidaData();
        readonly DataTable dtTable = new DataTable();
        public int TotalPaginas = 1;


        public FrmControleParcelamentos(string nomeUsuario)
        {
            InitializeComponent();
            AddColunasTabela();
            MapeiaNomeEmpresas();
            label6.Text = nomeUsuario;
        }

        private void MapeiaNomeEmpresas()
        {
            foreach (String nomeEmpresas in repositorioEmpresa.ListaNomeEmpresas())
            {
                comboBoxEmpresas.Items.Add(nomeEmpresas);
            }
            comboBoxEmpresas.SelectedIndex = 0;
        }

        private void BtnCarregar_Click(object sender, EventArgs e)
        {
            string[] stringEmpresa = comboBoxEmpresas.Text.Split('-');
            int codigoEmpresa = Convert.ToInt32(stringEmpresa[0]);
            int filialEmpresa = Convert.ToInt32(stringEmpresa[1]);
            string nomeEmpresa = stringEmpresa[2];
            var empresa = repositorioEmpresa.ObterEmpresaPorCodigoEFilial(codigoEmpresa, filialEmpresa);

            if (validaData.EhDataInvalida(maskedTextBoxPeriodoInicial.Text, maskedTextBoxPeriodoFinal.Text))
                return;

            DateTime periodoInicial = Convert.ToDateTime(maskedTextBoxPeriodoInicial.Text);
            DateTime periodoFinal = Convert.ToDateTime(maskedTextBoxPeriodoFinal.Text);

            
            if (codigoEmpresa.Equals(0) && filialEmpresa.Equals(0))
            {
                GridParcelamentosEmpresa(repositorioParcelamento.Get(x => x.Data >= periodoInicial && x.Data <= periodoFinal).OrderBy(x => x.Id));
                buttonEdit.Visible = true;
                buttonPesquisar.Visible = true;
                textBoxPesquisar.Visible = true;
                ColorirGrid();
            }
            else
            {
                GridParcelamentosEmpresa(repositorioParcelamento.Get(x => x.Data >= periodoInicial && x.Data <= periodoFinal && x.Empresa.First().Id.Equals(empresa.First().Id)).OrderBy(x => x.Id));
                buttonEdit.Visible = true;
                buttonPesquisar.Visible = true;
                textBoxPesquisar.Visible = true;
                ColorirGrid();
            }
            if (!repositorioParcelamento.ParcelamentosEmpresas.Any())
            {
                MessageBox.Show("Nenhum dado encontrado com os parâmetros informados!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return;
        }

        private void AddColunasTabela()
        {
            dtTable.Columns.Add("Id", typeof(int));
            dtTable.Columns.Add("Cód. Empresa", typeof(int));
            dtTable.Columns.Add("Filial", typeof(int));
            dtTable.Columns.Add("Nome Empresa", typeof(string));
            dtTable.Columns.Add("Cidade", typeof(string));
            dtTable.Columns.Add("Atividade", typeof(string));
            dtTable.Columns.Add("Regime", typeof(string));
            dtTable.Columns.Add("Parcelamento", typeof(string));
            dtTable.Columns.Add("Tipo", typeof(string));
            dtTable.Columns.Add("Nº Parcela", typeof(string));
            dtTable.Columns.Add("Data", typeof(DateTime));
            dtTable.Columns.Add("Status", typeof(string));
            dtTable.Columns.Add("Usuário", typeof(string));
        }

        private void GridParcelamentosEmpresa(IEnumerable<ParcelamentosEmpresa> parcelamentosEmpresas)
        {
            DadosDataTable(parcelamentosEmpresas);
            bindingSource1.DataSource = dtTable;
            dataGridView1.DataSource = bindingSource1;

            dataGridView1.Columns[0].Width = 30;
            dataGridView1.Columns[1].Width = 100;
            dataGridView1.Columns[2].Width = 30;
        }

        private void DadosDataTable(IEnumerable<ParcelamentosEmpresa> parcelamentosEmpresas)
        {

            dtTable.Clear();
            foreach (ParcelamentosEmpresa parcelamentosEmpresa in parcelamentosEmpresas)
            {
                dtTable.Rows.Add(parcelamentosEmpresa.Id, parcelamentosEmpresa.Empresa.First().Codigo, parcelamentosEmpresa.Empresa.First().Filial,
                   parcelamentosEmpresa.Empresa.First().NomeEmpresa, parcelamentosEmpresa.Cidade, parcelamentosEmpresa.Atividade, parcelamentosEmpresa.Regime,  
                   parcelamentosEmpresa.Parcelamento, parcelamentosEmpresa.Tipo, parcelamentosEmpresa.Parcela, parcelamentosEmpresa.Data,  parcelamentosEmpresa.Status, parcelamentosEmpresa.Usuario.First().NomeUsuario);
            }
        }

        private void ButtonAdicionar_Click(object sender, EventArgs e)
        {
            FrmAdicionarParcelamento frmControleParcelamentos = new FrmAdicionarParcelamento(label6.Text);
            frmControleParcelamentos.ShowDialog();
        }

        private void EmitirCSV(object sender, EventArgs e)
        {
            if (!PossuiItemNaGrid())
                return;

            SaveFileDialog salvar = new SaveFileDialog();
            salvar.FileName = "Relatorio_Parcelamentos_Empresa";

            Excel.Application App;
            Excel.Workbook WorkBook;
            Excel.Worksheet WorkSheet;
            object misValue = System.Reflection.Missing.Value;

            App = new Excel.Application();
            WorkBook = App.Workbooks.Add(misValue);
            WorkSheet = (Excel.Worksheet)WorkBook.Worksheets.get_Item(1);
            int linha = 1;
            int coluna = 0;

            DataGridViewCell cell = dataGridView1[coluna, linha];
            WorkSheet.Cells[linha, 1] = "ID";
            WorkSheet.Cells[linha, 2] = "COD";
            WorkSheet.Cells[linha, 3] = "FILIAL";
            WorkSheet.Cells[linha, 4] = "EMPRESA";
            WorkSheet.Cells[linha, 5] = "CIDADE";
            WorkSheet.Cells[linha, 6] = "ATIVIDADE";
            WorkSheet.Cells[linha, 7] = "REGIME";
            WorkSheet.Cells[linha, 8] = "PARCELAMENTO";
            WorkSheet.Cells[linha, 9] = "TIPO";
            WorkSheet.Cells[linha, 10] = "PARCELA";
            WorkSheet.Cells[linha, 11] = "DATA";
            WorkSheet.Cells[linha, 13] = "STATUS";

            var idParcelamento = WorkSheet.Cells[linha, 1];
            var codEmpresa = WorkSheet.Cells[linha, 2];
            var empresa = WorkSheet.Cells[linha, 3];
            var cidade = WorkSheet.Cells[linha, 4];
            var atividade = WorkSheet.Cells[linha, 5];
            var regime = WorkSheet.Cells[linha, 6];
            var parcelamento = WorkSheet.Cells[linha, 7];
            var tipo = WorkSheet.Cells[linha, 8];
            var parcela = WorkSheet.Cells[linha, 9];
            var data = WorkSheet.Cells[linha, 10];

            idParcelamento.Cells.Font.Bold = true;
            codEmpresa.Cells.Font.Bold = true;
            empresa.Cells.Font.Bold = true;
            cidade.Cells.Font.Bold = true;
            atividade.Cells.Font.Bold = true;
            regime.Cells.Font.Bold = true;
            parcelamento.Cells.Font.Bold = true;
            tipo.Cells.Font.Bold = true;
            parcela.Cells.Font.Bold = true;
            data.Cells.Font.Bold = true;

            for (linha = 1; linha <= dataGridView1.RowCount - 1; linha++)
            {
                for (coluna = 0; coluna <= dataGridView1.ColumnCount - 1; coluna++)
                {
                    DataGridViewCell cell2 = dataGridView1[coluna, linha - 1];
                    WorkSheet.Cells[linha + 1, coluna + 1] = cell2.Value;
                    var tes = WorkSheet.Cells[linha + 1, coluna + 1];
                    tes.Cells.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                }
            }

            Excel.Range celulas, celulasTitulo;

            celulasTitulo = WorkSheet.get_Range("A1:K1");
            celulas = WorkSheet.get_Range("A1:Z1000");

            celulasTitulo.Font.Bold = true;
            celulasTitulo.Font.Color = ColorTranslator.ToWin32(Color.White);
            celulasTitulo.Interior.Color = ColorTranslator.ToWin32(Color.DarkGray);
            celulas.HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
            celulas.EntireColumn.AutoFit();
            
            salvar.Title = "Exportar para Excel";
            salvar.Filter = "Arquivo do Excel *.xls | *.xls";
            salvar.ShowDialog();

            WorkBook.SaveAs(salvar.FileName, XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
            XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            WorkBook.Close(true, misValue, misValue);
            App.Quit();
        }



        public bool PossuiItemNaGrid()
        {
            if (dataGridView1.Rows.Count < 1)
            {
                MessageBox.Show("Nenhum dado para ser emitido!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void EmitirPDF(object sender, EventArgs e)
        {
            if (!PossuiItemNaGrid())
            {
                return;
            }

            if (repositorioParcelamento.QuantidadeEmpresas > 18)
                TotalPaginas += (int)Math.Ceiling(
                    (repositorioParcelamento.QuantidadeEmpresas - 18) / 20F);

            List<ParcelamentosEmpresa> listaParcelamentos = repositorioParcelamento.ParcelamentosEmpresas;

            BaseFont fonteBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            Document documento = new Document(PageSize.A4);
            documento.SetMargins(40, 40, 80, 40);
            documento.AddCreationDate();

            string caminhoArquivo = $@"{Path.GetTempFileName()}" + "relatorio.pdf";

            PdfWriter writer = PdfWriter.GetInstance(documento, new FileStream(caminhoArquivo, FileMode.Create));
            writer.PageEvent = new EventosPagina(TotalPaginas);
            documento.Open();

            Paragraph titulo = new Paragraph();
            titulo.Font = new Fonte(Fonte.FontFamily.TIMES_ROMAN, 16);
            titulo.Alignment = Element.ALIGN_CENTER;
            titulo.Add("RELATÓRIO DE PARCELAMENTO \n \n \n");

            PdfPTable tabela = new PdfPTable(9);

            float[] larguraColunas = { 0.8f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f};
            tabela.SetWidths(larguraColunas);
            tabela.SetWidths(larguraColunas);
            tabela.DefaultCell.BorderWidth = 0;
            tabela.WidthPercentage = 100;

            CriarCelulaTexto(tabela, "Cód.", 9, true);
            CriarCelulaTexto(tabela, "Nome Empresa", 9, true);
            CriarCelulaTexto(tabela, "Cidade", 9, true);
            CriarCelulaTexto(tabela, "Atividade", 9, true);
            CriarCelulaTexto(tabela, "Regime", 9, true);
            CriarCelulaTexto(tabela, "Parcelam.", 9, true);
            CriarCelulaTexto(tabela, "Tipo", 9, true);
            CriarCelulaTexto(tabela, "Parcela", 9, true);
            CriarCelulaTexto(tabela, "Data", 9, true);

            foreach (ParcelamentosEmpresa listaParcelamento in listaParcelamentos)
            {
                CriarCelulaTexto(tabela, listaParcelamento.Empresa.First().Codigo.ToString() + '-' + 
                    listaParcelamento.Empresa.First().Filial.ToString(), 8, false);
                CriarCelulaTexto(tabela, listaParcelamento.Empresa.First().NomeEmpresa.ToString(), 8, false);
                CriarCelulaTexto(tabela, listaParcelamento.Cidade.ToString(), 8, false);
                CriarCelulaTexto(tabela, listaParcelamento.Atividade.ToString(), 8, false);
                CriarCelulaTexto(tabela, listaParcelamento.Regime.ToString(), 8, false);
                CriarCelulaTexto(tabela, listaParcelamento.Parcelamento.ToString(), 8, false);
                CriarCelulaTexto(tabela, listaParcelamento.Tipo.ToString(), 8, false);
                CriarCelulaTexto(tabela, listaParcelamento.Parcela.ToString(), 8, false);
                CriarCelulaTexto(tabela, listaParcelamento.Data.Date.ToString().Substring(0, 10), 8, false);
            };

            documento.Add(titulo);
            documento.Add(tabela);
            documento.Close();

            System.Diagnostics.Process.Start(caminhoArquivo);
        }

        private static void CriarCelulaTexto(PdfPTable tabela, string texto, int tamanhoFonte, bool negrito,
           int alinhamentoHoriz = PdfPCell.ALIGN_LEFT, bool italico = false, int alturaCelula = 35)
        {
            {
                int estilo = Fonte.NORMAL;
                if (negrito && italico)
                {
                    estilo = Fonte.BOLDITALIC;
                }
                else if (negrito)
                {
                    estilo = Fonte.BOLD;
                }
                else if (italico)
                {
                    estilo = Fonte.ITALIC;
                }

                BaseFont fonteBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                Fonte fonte = new Fonte(fonteBase, tamanhoFonte,
                    estilo, BaseColor.BLACK);

                var bgColor = BaseColor.WHITE;
                if (tabela.Rows.Count % 2 == 1)
                    bgColor = new BaseColor(0.95f, 0.95f, 0.95f);

                PdfPCell celula = new PdfPCell(new Phrase(texto, fonte))
                {
                    HorizontalAlignment = alinhamentoHoriz,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    Border = 0,
                    BorderWidthBottom = 1,
                    FixedHeight = alturaCelula,
                    BackgroundColor = bgColor
                };
                tabela.AddCell(celula);
            }
        }

        private void Editar(object sender, EventArgs e)
        {
            var teste = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            IEnumerable<ParcelamentosEmpresa> parcelamentosEmpresa = repositorioParcelamento.Get(x => x.Id.Equals((int)dataGridView1.CurrentRow.Cells[0].Value));
            FrmEditarParcelamento frmControleParcelamentos = new FrmEditarParcelamento(parcelamentosEmpresa);
            frmControleParcelamentos.ShowDialog();
        }

        private void ColorirGrid()
        {
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[11].Value.ToString().Equals("ENVIADO"))
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.IndianRed;
                }
            }
        }

        private void Pesquisar(object sender, EventArgs e)
        {
            ParcelamentosEmpresa parcelamentosEmpresa = new ParcelamentosEmpresa();
            if (string.IsNullOrEmpty(textBoxPesquisar.Text))
            {
                BtnCarregar_Click(sender, e);
            }
            else
            {
                parcelamentosEmpresa.Parcela = textBoxPesquisar.Text;
                GridParcelamentosEmpresa(repositorioParcelamento.GetByParcela(parcelamentosEmpresa.Parcela));
                ColorirGrid();
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ColorirGrid();
        }
    }
}
