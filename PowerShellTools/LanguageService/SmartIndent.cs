﻿using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace PowerShellTools.LanguageService
{
    internal sealed class SmartIndent : ISmartIndent
    {
        private ITextView _textView;
        private PowerShellLanguageInfo _info;

        public SmartIndent(PowerShellLanguageInfo info, ITextView textView)
        {
            _info = info;
            _textView = textView;
        }

        public int? GetDesiredIndentation(ITextSnapshotLine line)
        {
            try
            {
                switch (_info.LangPrefs.IndentMode)
                {
                    case vsIndentStyle.vsIndentStyleNone:
                        return null;

                    case vsIndentStyle.vsIndentStyleDefault:
                        return GetDefaultIndentationImp(line);

                    case vsIndentStyle.vsIndentStyleSmart:
                        return GetSmartIndentationImp(line);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        private int? GetDefaultIndentationImp(ITextSnapshotLine line)
        {
            var lineNumber = line.LineNumber;

            if (lineNumber <= 1) return 0;

            var previousLine = _textView.TextSnapshot.GetLineFromLineNumber(lineNumber - 1);
            var lineChars = previousLine.GetText().ToCharArray();

            if (lineChars.Any() && lineChars[0] == '\t')
            {
                return 4;
            }

            for (int i = 0; i < lineChars.Length; i++)
            {
                if (!char.IsWhiteSpace(lineChars[i]))
                {
                    return i;
                }
            }

            return lineChars.Length;
        }

        private int? GetSmartIndentationImp(ITextSnapshotLine line)
        {
            var lineNumber = line.LineNumber;

            if (lineNumber <= 1) return 0;

            var previousLine = _textView.TextSnapshot.GetLineFromLineNumber(lineNumber - 1);
            var lineChars = previousLine.GetText().ToCharArray();

            if (lineChars.Any() && lineChars[0] == '\t')
            {
                return 4;
            }

            for (int i = 0; i < lineChars.Length; i++)
            {
                if (!char.IsWhiteSpace(lineChars[i]))
                {
                    return i;
                }
            }

            return lineChars.Length;
        }

        public void Dispose() { }
    }
}
