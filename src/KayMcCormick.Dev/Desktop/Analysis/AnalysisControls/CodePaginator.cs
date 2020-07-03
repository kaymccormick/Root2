#if false
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using KayMcCormick.Dev;
using RoslynCodeControls;

namespace AnalysisControls
{
    public class CodePaginator : DynamicDocumentPaginator
    {
        private readonly RoslynCodeControl _codeControlCodeControl;
        private int _pageCount=0;
        private readonly IDocumentPaginatorSource _source;
        private readonly bool _isPageCountValid = false;

        public CodePaginator(RoslynCodeControl codeControlCodeControl)
        {
            _codeControlCodeControl = codeControlCodeControl;
            _source = codeControlCodeControl;
            _pageCount = 1;
            _isPageCountValid = true;
        }

        /// <inheritdoc />
        public override DocumentPage GetPage(int pageNumber)
        {
            if (pageNumber > 0)
            {
                throw new AppInvalidOperationException();
            }
            var p = new DocumentPage(_codeControlCodeControl.Rectangle1);
            return p;
        }

        /// <inheritdoc />
        public CodePaginator()
        {
        }

        /// <inheritdoc />
        public override bool IsPageCountValid
        {
            get { return _isPageCountValid; }
        }

        /// <inheritdoc />
        public override int PageCount
        {
            get { return _pageCount; }
        }

        /// <inheritdoc />
        public override Size PageSize { get; set; } = new Size(96 * 8.5, 96 * 11);

        /// <inheritdoc />
        public override IDocumentPaginatorSource Source
        {
            get { return _source; }
        }

        /// <inheritdoc />
        public override void ComputePageCount()
        {
            base.ComputePageCount();
        }

        /// <inheritdoc />
        public override void ComputePageCountAsync(object userState)
        {
            base.ComputePageCountAsync(userState);
        }

        /// <inheritdoc />
        public override ContentPosition GetObjectPosition(object value)
        {
            return null;
        }

        /// <inheritdoc />
        public override bool IsBackgroundPaginationEnabled { get; set; }

        /// <inheritdoc />
        public override void ComputePageCountAsync()
        {
            base.ComputePageCountAsync();
        }

        /// <inheritdoc />
        public override void GetPageAsync(int pageNumber)
        {
            base.GetPageAsync(pageNumber);
        }

        /// <inheritdoc />
        public override int GetPageNumber(ContentPosition contentPosition)
        {
            return 0;
        }

        /// <inheritdoc />
        public override ContentPosition GetPagePosition(DocumentPage page)
        {
            return null;
        }
    }
}

#endif