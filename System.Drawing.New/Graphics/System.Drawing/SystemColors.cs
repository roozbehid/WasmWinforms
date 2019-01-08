//
// System.Drawing.SystemColors
//
// Copyright (C) 2002 Ximian, Inc (http://www.ximian.com)
// Copyright (C) 2004-2005, 2007 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Authors:
//	Gonzalo Paniagua Javier (gonzalo@ximian.com)
//	Peter Dennis Bartok (pbartok@novell.com)
//	Sebastien Pouliot  <sebastien@ximian.com>
//

namespace System.Drawing {

	public sealed class SystemColors {

		private SystemColors ()
		{
		}

		static public Color_ ActiveBorder {
			get { return KnownColors.FromKnownColor (KnownColor.ActiveBorder); }
		}

		static public Color_ ActiveCaption {
			get { return KnownColors.FromKnownColor (KnownColor.ActiveCaption); }
		}

		static public Color_ ActiveCaptionText {
			get { return KnownColors.FromKnownColor (KnownColor.ActiveCaptionText); }
		}

		static public Color_ AppWorkspace {
			get { return KnownColors.FromKnownColor (KnownColor.AppWorkspace); }
		}

		static public Color_ Control {
			get { return KnownColors.FromKnownColor (KnownColor.Control); }
		}

		static public Color_ ControlDark {
			get { return KnownColors.FromKnownColor (KnownColor.ControlDark); }
		}

		static public Color_ ControlDarkDark {
			get { return KnownColors.FromKnownColor (KnownColor.ControlDarkDark); }
		}

		static public Color_ ControlLight {
			get { return KnownColors.FromKnownColor (KnownColor.ControlLight); }
		}

		static public Color_ ControlLightLight {
			get { return KnownColors.FromKnownColor (KnownColor.ControlLightLight); }
		}

		static public Color_ ControlText {
			get { return KnownColors.FromKnownColor (KnownColor.ControlText); }
		}

		static public Color_ Desktop {
			get { return KnownColors.FromKnownColor (KnownColor.Desktop); }
		}

		static public Color_ GrayText {
			get { return KnownColors.FromKnownColor (KnownColor.GrayText); }
		}

		static public Color_ Highlight {
			get { return KnownColors.FromKnownColor (KnownColor.Highlight); }
		}

		static public Color_ HighlightText {
			get { return KnownColors.FromKnownColor (KnownColor.HighlightText); }
		}

		static public Color_ HotTrack {
			get { return KnownColors.FromKnownColor (KnownColor.HotTrack); }
		}

		static public Color_ InactiveBorder {
			get { return KnownColors.FromKnownColor (KnownColor.InactiveBorder); }
		}

		static public Color_ InactiveCaption {
			get { return KnownColors.FromKnownColor (KnownColor.InactiveCaption); }
		}

		static public Color_ InactiveCaptionText {
			get { return KnownColors.FromKnownColor (KnownColor.InactiveCaptionText); }
		}

		static public Color_ Info {
			get { return KnownColors.FromKnownColor (KnownColor.Info); }
		}

		static public Color_ InfoText {
			get { return KnownColors.FromKnownColor (KnownColor.InfoText); }
		}

		static public Color_ Menu {
			get { return KnownColors.FromKnownColor (KnownColor.Menu); }
		}

		static public Color_ MenuText {
			get { return KnownColors.FromKnownColor (KnownColor.MenuText); }
		}

		static public Color_ ScrollBar {
			get { return KnownColors.FromKnownColor (KnownColor.ScrollBar); }
		}

		static public Color_ Window {
			get { return KnownColors.FromKnownColor (KnownColor.Window); }
		}

		static public Color_ WindowFrame {
			get { return KnownColors.FromKnownColor (KnownColor.WindowFrame); }
		}

		static public Color_ WindowText {
			get { return KnownColors.FromKnownColor (KnownColor.WindowText); }
		}
#if NET_2_0
		static public Color_ ButtonFace {
			get { return KnownColors.FromKnownColor (KnownColor.ButtonFace); }
		}

		static public Color_ ButtonHighlight {
			get { return KnownColors.FromKnownColor (KnownColor.ButtonHighlight); }
		}

		static public Color_ ButtonShadow {
			get { return KnownColors.FromKnownColor (KnownColor.ButtonShadow); }
		}

		static public Color_ GradientActiveCaption {
			get { return KnownColors.FromKnownColor (KnownColor.GradientActiveCaption); }
		}

		static public Color_ GradientInactiveCaption {
			get { return KnownColors.FromKnownColor (KnownColor.GradientInactiveCaption); }
		}

		static public Color_ MenuBar {
			get { return KnownColors.FromKnownColor (KnownColor.MenuBar); }
		}

		static public Color_ MenuHighlight {
			get { return KnownColors.FromKnownColor (KnownColor.MenuHighlight); }
		}
#endif
	}
}
