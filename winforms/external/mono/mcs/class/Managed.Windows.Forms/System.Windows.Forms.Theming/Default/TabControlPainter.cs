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
// Copyright (c) 2007 Novell, Inc.
//
// Authors:
//	Andreia Gaita (avidigal@novell.com)

using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms.Theming.Default
{
	/// <summary>
	/// Summary description for TabControl.
	/// </summary>
	internal class TabControlPainter {
		protected SystemResPool ResPool { get { return ThemeEngine.Current.ResPool; } }

		#region private

		private Size_	defaultItemSize;
		private Point_	defaultPadding;
		private int		minimumTabWidth;
		private Rectangle_ selectedTabDelta;

		private Point_	tabPanelOffset;
		
		private int		selectedSpacing;

		private Size_	rowSpacingNormal;
		private Size_	rowSpacingButtons;
		private Size_	rowSpacingFlatButtons;
		private int		scrollerWidth;
		private Point_	focusRectSpacing;
		private Rectangle_ tabPageSpacing;
		private int		colSpacing;
		private int		flatButtonSpacing;

		private Point_	imagePadding;

		private StringFormat defaultFormatting;

		private Rectangle_ borderThickness;

		#endregion

		#region Properties

		public virtual Size_ DefaultItemSize {
			get { return defaultItemSize; }
			set { defaultItemSize = value; }
		}

		public virtual Point_ DefaultPadding {
			get { return defaultPadding; }
			set { defaultPadding = value; }
		}

		public virtual int MinimumTabWidth {
			get { return minimumTabWidth; }
			set { minimumTabWidth = value; }
		}

		public virtual Rectangle_ SelectedTabDelta {
			get { return selectedTabDelta; }
			set { selectedTabDelta = value; }
		}

		public virtual Point_ TabPanelOffset {
			get { return tabPanelOffset; }
			set { tabPanelOffset = value; }
		}

		public virtual int SelectedSpacing {
			get { return selectedSpacing; }
			set { selectedSpacing = value; }
		}

		public virtual Size_ RowSpacingNormal { 
			get { return rowSpacingNormal; }
			set { rowSpacingNormal = value; }
		}

		public virtual Size_ RowSpacingButtons { 
			get { return rowSpacingButtons; }
			set { rowSpacingButtons = value; }
		}

		public virtual Size_ RowSpacingFlatButtons { 
			get { return rowSpacingFlatButtons; }
			set { rowSpacingFlatButtons = value; }
		}

		public virtual Point_ FocusRectSpacing {
			get { return focusRectSpacing; }
			set { focusRectSpacing = value; }
		}

		public virtual int ColSpacing { 
			get { return colSpacing; }
			set { colSpacing = value; }
		}

		public virtual int FlatButtonSpacing { 
			get { return flatButtonSpacing; }
			set { flatButtonSpacing = value; }
		}

		public virtual Rectangle_ TabPageSpacing {
			get { return tabPageSpacing; }
			set { tabPageSpacing = value; }
		}

		public virtual Point_ ImagePadding {
			get { return imagePadding; }
			set { imagePadding = value; }
		}

		public virtual StringFormat DefaultFormatting {
			get { return defaultFormatting; }
			set { defaultFormatting = value; }
		}

		public virtual Rectangle_ BorderThickness {
			get { return borderThickness; }
			set { borderThickness = value; }
		}

		public virtual int ScrollerWidth {
			get { return scrollerWidth; }
			set { scrollerWidth = value; }
		}

		public virtual Size_ RowSpacing (System.Windows.Forms.TabControl tab) {
			switch (tab.Appearance) {
				case TabAppearance.Normal:
					return rowSpacingNormal;
				case TabAppearance.Buttons:
					return rowSpacingButtons;
				case TabAppearance.FlatButtons:
					return rowSpacingFlatButtons;
				default:
					throw new Exception ("Invalid Appearance value: " + tab.Appearance);
			}
		}
		#endregion

		public TabControlPainter ()
		{
			defaultItemSize = new Size_ (42, 16);
			defaultPadding = new Point_ (6, 3);
			selectedTabDelta = new Rectangle_ (2, 2, 4, 3);
			selectedSpacing = 0;

			rowSpacingNormal = new Size_ (0, 0);
			rowSpacingButtons = new Size_ (3, 3);
			rowSpacingFlatButtons = new Size_ (9, 3);
			
			colSpacing = 0;

			minimumTabWidth = 42;
			scrollerWidth = 17;
			focusRectSpacing = new Point_ (2, 2);
			tabPanelOffset = new Point_ (4, 0);
			flatButtonSpacing = 8;
			tabPageSpacing = new Rectangle_ (4, 2, 3, 4);

			imagePadding = new Point_ (2, 3);

			defaultFormatting = new StringFormat();
			// Horizontal Alignment is handled in the Draw method
			defaultFormatting.Alignment = StringAlignment.Near;
			defaultFormatting.LineAlignment = StringAlignment.Center;
			defaultFormatting.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.NoClip;
			defaultFormatting.HotkeyPrefix = HotkeyPrefix.Show;

			borderThickness = new Rectangle_ (1, 1, 2, 2);
		}

		public virtual Rectangle_ GetLeftScrollRect (System.Windows.Forms.TabControl tab)
		{
			switch (tab.Alignment) {
				case TabAlignment.Top:
					return new Rectangle_ (tab.ClientRectangle.Right - (scrollerWidth * 2), tab.ClientRectangle.Top + 1, scrollerWidth, scrollerWidth);
				default:
					Rectangle_ panel_rect = GetTabPanelRect (tab);
					return new Rectangle_ (tab.ClientRectangle.Right - (scrollerWidth * 2), panel_rect.Bottom + 2, scrollerWidth, scrollerWidth);
			}
		}

		public virtual Rectangle_ GetRightScrollRect (System.Windows.Forms.TabControl tab)
		{
			switch (tab.Alignment) {
				case TabAlignment.Top:
					return new Rectangle_ (tab.ClientRectangle.Right - (scrollerWidth), tab.ClientRectangle.Top + 1, scrollerWidth, scrollerWidth);
				default:
					Rectangle_ panel_rect = GetTabPanelRect (tab);
					return new Rectangle_ (tab.ClientRectangle.Right - (scrollerWidth), panel_rect.Bottom + 2, scrollerWidth, scrollerWidth);
			}
		}

		public Rectangle_ GetDisplayRectangle (System.Windows.Forms.TabControl tab)
		{
			Rectangle_ ext = GetTabPanelRect (tab);
			// Account for border size
			return new Rectangle_ (ext.Left + tabPageSpacing.X, ext.Top + tabPageSpacing.Y, 
				ext.Width - tabPageSpacing.X - tabPageSpacing.Width, ext.Height - tabPageSpacing.Y - tabPageSpacing.Height);
		}
		
		public Rectangle_ GetTabPanelRect (System.Windows.Forms.TabControl tab)
		{	
			// Offset the tab page (panel) from the top corner
			Rectangle_ res = tab.ClientRectangle;

			if (tab.TabCount == 0)
				return res;

			int spacing = RowSpacing (tab).Height;
			int tabOffset = (tab.ItemSize.Height + spacing - selectedTabDelta.Height) * tab.RowCount + selectedTabDelta.Y;
			switch (tab.Alignment) {
				case TabAlignment.Top:
					res.Y += tabOffset;
					res.Height -= tabOffset;
					break;
				case TabAlignment.Bottom:
					res.Height -= tabOffset;
					break;
				case TabAlignment.Left:
					res.X += tabOffset;
					res.Width -= tabOffset;
					break;
				case TabAlignment.Right:
					res.Width -= tabOffset;
					break;
			}

			return res;
		}

		public virtual void Draw (Graphics dc, Rectangle_ area, TabControl tab)
		{
			DrawBackground (dc, area, tab);

			int start = 0;
			int end = tab.TabPages.Count;
			int delta = 1;

			if (tab.Alignment == TabAlignment.Top) {
				start = end;
				end = 0;
				delta = -1;
			}

			if (tab.SizeMode == TabSizeMode.Fixed)
				defaultFormatting.Alignment = StringAlignment.Center;
			else
				defaultFormatting.Alignment = StringAlignment.Near;

			int counter = start;
			for (; counter != end; counter += delta) {
				for (int i = tab.SliderPos; i < tab.TabPages.Count; i++) {
					if (i == tab.SelectedIndex)
						continue;
					if (counter != tab.TabPages[i].Row)
						continue;
					Rectangle_ rect = tab.GetTabRect (i);
					if (!rect.IntersectsWith (area))
						continue;
					DrawTab (dc, tab.TabPages[i], tab, rect, false);
				}
			}

			if (tab.SelectedIndex != -1 && tab.SelectedIndex >= tab.SliderPos) {
				Rectangle_ rect = tab.GetTabRect (tab.SelectedIndex);
				if (rect.IntersectsWith (area))
					DrawTab (dc, tab.TabPages[tab.SelectedIndex], tab, rect, true);
			}

			if (tab.ShowSlider) {
				Rectangle_ right = GetRightScrollRect (tab);
				Rectangle_ left = GetLeftScrollRect (tab);
				DrawScrollButton (dc, right, area, ScrollButton.Right, tab.RightSliderState);
				DrawScrollButton (dc, left, area, ScrollButton.Left, tab.LeftSliderState);
			}
		}

		protected virtual void DrawScrollButton (Graphics dc, Rectangle_ bounds, Rectangle_ clippingArea, ScrollButton button, PushButtonState state)
		{
			ControlPaint.DrawScrollButton (dc, bounds, button, GetButtonState (state));
		}

		static ButtonState GetButtonState (PushButtonState state)
		{
			switch (state) {
			case PushButtonState.Pressed:
				return ButtonState.Pushed;
			default:
				return ButtonState.Normal;
			}
		}

		protected virtual void DrawBackground (Graphics dc, Rectangle_ area, TabControl tab)
		{
			Brush brush = SystemBrushes.Control;
			dc.FillRectangle (brush, area);
			Rectangle_ panel_rect = GetTabPanelRect (tab);

			if (tab.Appearance == TabAppearance.Normal) {
				ControlPaint.DrawBorder3D (dc, panel_rect, Border3DStyle.RaisedInner, Border3DSide.Left | Border3DSide.Top);
				ControlPaint.DrawBorder3D (dc, panel_rect, Border3DStyle.Raised, Border3DSide.Right | Border3DSide.Bottom);
			}
		}

		protected virtual int DrawTab (Graphics dc, System.Windows.Forms.TabPage page, System.Windows.Forms.TabControl tab, Rectangle_ bounds, bool is_selected)
		{
			Rectangle_ interior;
			int res = bounds.Width;

			dc.FillRectangle (ResPool.GetSolidBrush (tab.BackColor), bounds);

			if (tab.Appearance == TabAppearance.Buttons || tab.Appearance == TabAppearance.FlatButtons) {
				// Separators
				if (tab.Appearance == TabAppearance.FlatButtons) {
					int width = bounds.Width;
					bounds.Width += (flatButtonSpacing - 2);
					res = bounds.Width;
					if (tab.Alignment == TabAlignment.Top || tab.Alignment == TabAlignment.Bottom)
						ThemeEngine.Current.CPDrawBorder3D (dc, bounds, Border3DStyle.Etched, Border3DSide.Right);
					else
						ThemeEngine.Current.CPDrawBorder3D (dc, bounds, Border3DStyle.Etched, Border3DSide.Top);
					bounds.Width = width;
				}

				if (is_selected) {
					ThemeEngine.Current.CPDrawBorder3D (dc, bounds, Border3DStyle.Sunken, Border3DSide.Left | Border3DSide.Right | Border3DSide.Top | Border3DSide.Bottom);
				} else if (tab.Appearance != TabAppearance.FlatButtons) {
					ThemeEngine.Current.CPDrawBorder3D (dc, bounds, Border3DStyle.Raised, Border3DSide.Left | Border3DSide.Right | Border3DSide.Top | Border3DSide.Bottom);
				}


			} else {
				CPColor cpcolor = ResPool.GetCPColor (tab.BackColor);

				Pen light = ResPool.GetPen (cpcolor.LightLight);

				switch (tab.Alignment) {

					case TabAlignment.Top:

						dc.DrawLine (light, bounds.Left, bounds.Bottom - 1, bounds.Left, bounds.Top + 3);
						dc.DrawLine (light, bounds.Left, bounds.Top + 3, bounds.Left + 2, bounds.Top);
						dc.DrawLine (light, bounds.Left + 2, bounds.Top, bounds.Right - 3, bounds.Top);

						dc.DrawLine (SystemPens.ControlDark, bounds.Right - 2, bounds.Top + 1, bounds.Right - 2, bounds.Bottom - 1);
						dc.DrawLine (SystemPens.ControlDarkDark, bounds.Right - 2, bounds.Top + 1, bounds.Right - 1, bounds.Top + 2);
						dc.DrawLine (SystemPens.ControlDarkDark, bounds.Right - 1, bounds.Top + 2, bounds.Right - 1, bounds.Bottom - 1);
						break;

					case TabAlignment.Bottom:

						dc.DrawLine (light, bounds.Left, bounds.Top, bounds.Left, bounds.Bottom - 2);
						dc.DrawLine (light, bounds.Left, bounds.Bottom - 2, bounds.Left + 3, bounds.Bottom);
						
						dc.DrawLine (SystemPens.ControlDarkDark, bounds.Left + 3, bounds.Bottom, bounds.Right - 3, bounds.Bottom);
						dc.DrawLine (SystemPens.ControlDark, bounds.Left + 3, bounds.Bottom - 1, bounds.Right - 3, bounds.Bottom - 1);

						dc.DrawLine (SystemPens.ControlDark, bounds.Right - 2, bounds.Bottom - 1, bounds.Right - 2, bounds.Top + 1);
						dc.DrawLine (SystemPens.ControlDarkDark, bounds.Right - 2, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 2);
						dc.DrawLine (SystemPens.ControlDarkDark, bounds.Right - 1, bounds.Bottom - 2, bounds.Right - 1, bounds.Top + 1);

						break;

					case TabAlignment.Left:

						dc.DrawLine (light, bounds.Left - 2, bounds.Top, bounds.Right, bounds.Top);
						dc.DrawLine (light, bounds.Left, bounds.Top + 2, bounds.Left - 2, bounds.Top);
						dc.DrawLine (light, bounds.Left, bounds.Top + 2, bounds.Left, bounds.Bottom - 2);

						dc.DrawLine (SystemPens.ControlDark, bounds.Left, bounds.Bottom - 2, bounds.Left + 2, bounds.Bottom - 1);

						dc.DrawLine (SystemPens.ControlDark, bounds.Left + 2, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
						dc.DrawLine (SystemPens.ControlDarkDark, bounds.Left + 2, bounds.Bottom, bounds.Right, bounds.Bottom);

						break;

					default: // TabAlignment.Right

						dc.DrawLine (light, bounds.Left, bounds.Top, bounds.Right - 3, bounds.Top);
						dc.DrawLine (light, bounds.Right - 3, bounds.Top, bounds.Right, bounds.Top + 3);

						dc.DrawLine (SystemPens.ControlDark, bounds.Right - 1, bounds.Top + 1, bounds.Right - 1, bounds.Bottom - 1);
						dc.DrawLine (SystemPens.ControlDark, bounds.Left, bounds.Bottom - 1, bounds.Right - 2, bounds.Bottom - 1);

						dc.DrawLine (SystemPens.ControlDarkDark, bounds.Right, bounds.Top + 3, bounds.Right, bounds.Bottom - 3);
						dc.DrawLine (SystemPens.ControlDarkDark, bounds.Left, bounds.Bottom, bounds.Right - 3, bounds.Bottom);

						break;
				}
			}

			Point_ padding = tab.Padding;
			interior = new Rectangle_ (bounds.Left + padding.X - 1, // substract a little offset
				bounds.Top + padding.Y,
				bounds.Width - (padding.X * 2), 
				bounds.Height - (padding.Y * 2));

			if (tab.DrawMode == TabDrawMode.Normal && page.Text != null) {
				if (tab.Alignment == TabAlignment.Left) {
					dc.TranslateTransform (bounds.Left, bounds.Bottom);
					dc.RotateTransform (-90);
					dc.DrawString (page.Text, tab.Font,
						SystemBrushes.ControlText, 
						tab.Padding.X - 2, // drawstring adds some extra unwanted leading spaces, so trimming
						tab.Padding.Y,
						defaultFormatting);
					dc.ResetTransform ();
				} else if (tab.Alignment == TabAlignment.Right) {
					dc.TranslateTransform (bounds.Right, bounds.Top);
					dc.RotateTransform (90);
					dc.DrawString (page.Text, tab.Font,
						SystemBrushes.ControlText, 
						tab.Padding.X - 2, // drawstring adds some extra unwanted leading spaces, so trimming
						tab.Padding.Y,
						defaultFormatting);
					dc.ResetTransform ();
				} else {
					Rectangle_ str_rect = interior;

					if (is_selected) {
						// Reduce the interior Size_ to match the inner Size_ of non-selected tabs
						str_rect.X += selectedTabDelta.X;
						str_rect.Y += selectedTabDelta.Y;
						str_rect.Width -= selectedTabDelta.Width;
						str_rect.Height -= selectedTabDelta.Height;

						str_rect.Y -= selectedTabDelta.Y; // Move up the text / image of the selected tab
					}

					if (tab.ImageList != null && page.ImageIndex >= 0 && page.ImageIndex < tab.ImageList.Images.Count) {
						int image_x;
						if (tab.SizeMode != TabSizeMode.Fixed) {
							image_x = str_rect.X;
						}
						else {
							image_x = str_rect.X + (str_rect.Width - tab.ImageList.ImageSize.Width) / 2;
							if (page.Text != null) {
								SizeF_ textSize = dc.MeasureString(page.Text, page.Font, str_rect.Size);
								image_x -= (int)(textSize.Width / 2);
							}
						}
						int image_y = str_rect.Y + (str_rect.Height - tab.ImageList.ImageSize.Height) / 2;
						tab.ImageList.Draw (dc, new Point_ (image_x, image_y), page.ImageIndex);
						str_rect.X += tab.ImageList.ImageSize.Width + 2;
						str_rect.Width -= tab.ImageList.ImageSize.Width + 2;
					}
					dc.DrawString (page.Text, tab.Font,
						SystemBrushes.ControlText,
						str_rect, 
						defaultFormatting);

				}
			} else if (page.Text != null) {
				DrawItemState state = DrawItemState.None;
				if (page == tab.SelectedTab)
					state |= DrawItemState.Selected;
				DrawItemEventArgs e = new DrawItemEventArgs (dc,
					tab.Font, bounds, tab.IndexForTabPage (page),
					state, page.ForeColor, page.BackColor);
				tab.OnDrawItemInternal (e);
				return res;
			}

			// TabControl ignores the value of ShowFocusCues
			if (page.Parent.Focused && is_selected) {
				Rectangle_ focus_rect = bounds;
				focus_rect.Inflate (-2, -2);
				ThemeEngine.Current.CPDrawFocusRectangle (dc, focus_rect, tab.BackColor, tab.ForeColor);
			}

			return res;
		}

		public virtual bool HasHotElementStyles (TabControl tabControl) {
			return false;
		}
	}
}
