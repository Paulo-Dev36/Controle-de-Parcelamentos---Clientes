using iTextSharp.text;
using iTextSharp.text.pdf;
using System;

namespace RelatoriosAtendimento
{
    class EventosPagina : PdfPageEventHelper
    {
        private PdfContentByte Wdc;
        private BaseFont FonteBaseRodape { get; set; }
        private Font FonteRodape { get; set; }
        int totalPaginas = 0;

        public EventosPagina(int paginasTotal)
        {
            FonteBaseRodape = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            FonteRodape = new Font(FonteBaseRodape, 8f, Font.NORMAL, BaseColor.BLACK);
            this.totalPaginas = paginasTotal;
        }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
            this.Wdc = writer.DirectContent;

        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            AdicionaDataDaEmissao(writer, document);
            AdicionarNumerosDasPaginas(writer, document);
            AdicionarLogoNoCabecalho(writer, document);
        }

        private void AdicionaDataDaEmissao(PdfWriter writer, Document document)
        {
            var textoMomentoGeracao = $@"Gerado em {DateTime.Now.ToShortDateString()} às {DateTime.Now.ToShortTimeString()}";

            Wdc.BeginText();
            Wdc.SetFontAndSize(FonteRodape.BaseFont, FonteRodape.Size);
            Wdc.SetTextMatrix(document.LeftMargin, document.BottomMargin * 0.75f);
            Wdc.ShowText(textoMomentoGeracao);
            Wdc.EndText();
        }

        private void AdicionarNumerosDasPaginas(PdfWriter writer, Document document)
        {
            int paginaAtual = writer.PageNumber;
            var textoPaginacao = $"Página {paginaAtual} de {totalPaginas}";

            float larguraTextoPaginacao = FonteBaseRodape.GetWidthPoint(textoPaginacao, FonteRodape.Size);

            var tamanhoPagina = document.PageSize;

            Wdc.BeginText();
            Wdc.SetFontAndSize(FonteRodape.BaseFont, FonteRodape.Size);
            Wdc.SetTextMatrix(tamanhoPagina.Width - document.RightMargin - larguraTextoPaginacao, document.BottomMargin * 0.75f);
            Wdc.ShowText(textoPaginacao);
            Wdc.EndText();
        }

        private void AdicionarLogoNoCabecalho(PdfWriter writer, Document document)
        {
            string caminhoImagem = "https://www.wlcontab.com.br/wp-content/uploads/2022/06/Marca-WL-contab_finalizado02.png";
            Image logo = Image.GetInstance(caminhoImagem);
            float razaoAlturaLargura = logo.Width / logo.Height;
            float alturaLogo = 32;
            float larguraLogo = alturaLogo * razaoAlturaLargura;

            logo.ScaleToFit(larguraLogo, alturaLogo);
            var margemEsquerda = document.PageSize.Width - document.RightMargin - larguraLogo;
            var margemTopo = 792;


            logo.SetAbsolutePosition(margemEsquerda, margemTopo);
            writer.DirectContent.AddImage(logo, false);
        }
    }
}

