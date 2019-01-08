//
// ProfessionalColorTable.cs
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
// Copyright (c) 2006 Novell, Inc.
//
// Authors:
//	Jonathan Pobst (monkey@jpobst.com)
//

using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	public class ProfessionalColorTable
	{
		#region Private Variables
		private bool use_system_colors = false;

		private Color_ button_checked_gradient_begin;
		private Color_ button_checked_gradient_end;
		private Color_ button_checked_gradient_middle;
		private Color_ button_checked_highlight;
		private Color_ button_checked_highlight_border;
		private Color_ button_pressed_border;
		private Color_ button_pressed_gradient_begin;
		private Color_ button_pressed_gradient_end;
		private Color_ button_pressed_gradient_middle;
		private Color_ button_pressed_highlight;
		private Color_ button_pressed_highlight_border;
		private Color_ button_selected_border;
		private Color_ button_selected_gradient_begin;
		private Color_ button_selected_gradient_end;
		private Color_ button_selected_gradient_middle;
		private Color_ button_selected_highlight;
		private Color_ button_selected_highlight_border;
		private Color_ check_background;
		private Color_ check_pressed_background;
		private Color_ check_selected_background;
		private Color_ grip_dark;
		private Color_ grip_light;
		private Color_ image_margin_gradient_begin;
		private Color_ image_margin_gradient_end;
		private Color_ image_margin_gradient_middle;
		private Color_ image_margin_revealed_gradient_begin;
		private Color_ image_margin_revealed_gradient_end;
		private Color_ image_margin_revealed_gradient_middle;
		private Color_ menu_border;
		private Color_ menu_item_border;
		private Color_ menu_item_pressed_gradient_begin;
		private Color_ menu_item_pressed_gradient_end;
		private Color_ menu_item_pressed_gradient_middle;
		private Color_ menu_item_selected;
		private Color_ menu_item_selected_gradient_begin;
		private Color_ menu_item_selected_gradient_end;
		private Color_ menu_strip_gradient_begin;
		private Color_ menu_strip_gradient_end;
		private Color_ overflow_button_gradient_begin;
		private Color_ overflow_button_gradient_end;
		private Color_ overflow_button_gradient_middle;
		private Color_ rafting_container_gradient_begin;
		private Color_ rafting_container_gradient_end;
		private Color_ separator_dark;
		private Color_ separator_light;
		private Color_ status_strip_gradient_begin;
		private Color_ status_strip_gradient_end;
		private Color_ tool_strip_border;
		private Color_ tool_strip_content_panel_gradient_begin;
		private Color_ tool_strip_content_panel_gradient_end;
		private Color_ tool_strip_drop_down_background;
		private Color_ tool_strip_gradient_begin;
		private Color_ tool_strip_gradient_end;
		private Color_ tool_strip_gradient_middle;
		private Color_ tool_strip_panel_gradient_begin;
		private Color_ tool_strip_panel_gradient_end;
		#endregion

		#region Public Constructor
		public ProfessionalColorTable ()
		{
			CalculateColors ();
		}
		#endregion

		#region Public Properties
		public virtual Color_ ButtonCheckedGradientBegin { get { return this.button_checked_gradient_begin; } }
		public virtual Color_ ButtonCheckedGradientEnd { get { return this.button_checked_gradient_end; } }
		public virtual Color_ ButtonCheckedGradientMiddle { get { return this.button_checked_gradient_middle; } }
		public virtual Color_ ButtonCheckedHighlight { get { return this.button_checked_highlight; } }
		public virtual Color_ ButtonCheckedHighlightBorder { get { return this.button_checked_highlight_border; } }
		public virtual Color_ ButtonPressedBorder { get { return this.button_pressed_border; } }
		public virtual Color_ ButtonPressedGradientBegin { get { return this.button_pressed_gradient_begin; } }
		public virtual Color_ ButtonPressedGradientEnd { get { return this.button_pressed_gradient_end; } }
		public virtual Color_ ButtonPressedGradientMiddle { get { return this.button_pressed_gradient_middle; } }
		public virtual Color_ ButtonPressedHighlight { get { return this.button_pressed_highlight; } }
		public virtual Color_ ButtonPressedHighlightBorder { get { return this.button_pressed_highlight_border; } }
		public virtual Color_ ButtonSelectedBorder { get { return this.button_selected_border; } }
		public virtual Color_ ButtonSelectedGradientBegin { get { return this.button_selected_gradient_begin; } }
		public virtual Color_ ButtonSelectedGradientEnd { get { return this.button_selected_gradient_end; } }
		public virtual Color_ ButtonSelectedGradientMiddle { get { return this.button_selected_gradient_middle; } }
		public virtual Color_ ButtonSelectedHighlight { get { return this.button_selected_highlight; } }
		public virtual Color_ ButtonSelectedHighlightBorder { get { return this.button_selected_highlight_border; } }
		public virtual Color_ CheckBackground { get { return this.check_background; } }
		public virtual Color_ CheckPressedBackground { get { return this.check_pressed_background; } }
		public virtual Color_ CheckSelectedBackground { get { return this.check_selected_background; } }
		public virtual Color_ GripDark { get { return this.grip_dark; } }
		public virtual Color_ GripLight { get { return this.grip_light; } }
		public virtual Color_ ImageMarginGradientBegin { get { return this.image_margin_gradient_begin; } }
		public virtual Color_ ImageMarginGradientEnd { get { return this.image_margin_gradient_end; } }
		public virtual Color_ ImageMarginGradientMiddle { get { return this.image_margin_gradient_middle; } }
		public virtual Color_ ImageMarginRevealedGradientBegin { get { return this.image_margin_revealed_gradient_begin; } }
		public virtual Color_ ImageMarginRevealedGradientEnd { get { return this.image_margin_revealed_gradient_end; } }
		public virtual Color_ ImageMarginRevealedGradientMiddle { get { return this.image_margin_revealed_gradient_middle; } }
		public virtual Color_ MenuBorder { get { return this.menu_border; } }
		public virtual Color_ MenuItemBorder { get { return this.menu_item_border; } }
		public virtual Color_ MenuItemPressedGradientBegin { get { return this.menu_item_pressed_gradient_begin; } }
		public virtual Color_ MenuItemPressedGradientEnd { get { return this.menu_item_pressed_gradient_end; } }
		public virtual Color_ MenuItemPressedGradientMiddle { get { return this.menu_item_pressed_gradient_middle; } }
		public virtual Color_ MenuItemSelected { get { return this.menu_item_selected; } }
		public virtual Color_ MenuItemSelectedGradientBegin { get { return this.menu_item_selected_gradient_begin; } }
		public virtual Color_ MenuItemSelectedGradientEnd { get { return this.menu_item_selected_gradient_end; } }
		public virtual Color_ MenuStripGradientBegin { get { return this.menu_strip_gradient_begin; } }
		public virtual Color_ MenuStripGradientEnd { get { return this.menu_strip_gradient_end; } }
		public virtual Color_ OverflowButtonGradientBegin { get { return this.overflow_button_gradient_begin; } }
		public virtual Color_ OverflowButtonGradientEnd { get { return this.overflow_button_gradient_end; } }
		public virtual Color_ OverflowButtonGradientMiddle { get { return this.overflow_button_gradient_middle; } }
		public virtual Color_ RaftingContainerGradientBegin { get { return this.rafting_container_gradient_begin; } }
		public virtual Color_ RaftingContainerGradientEnd { get { return this.rafting_container_gradient_end; } }
		public virtual Color_ SeparatorDark { get { return this.separator_dark; } }
		public virtual Color_ SeparatorLight { get { return this.separator_light; } }
		public virtual Color_ StatusStripGradientBegin { get { return this.status_strip_gradient_begin; } }
		public virtual Color_ StatusStripGradientEnd { get { return this.status_strip_gradient_end; } }
		public virtual Color_ ToolStripBorder { get { return this.tool_strip_border; } }
		public virtual Color_ ToolStripContentPanelGradientBegin { get { return this.tool_strip_content_panel_gradient_begin; } }
		public virtual Color_ ToolStripContentPanelGradientEnd { get { return this.tool_strip_content_panel_gradient_end; } }
		public virtual Color_ ToolStripDropDownBackground { get { return this.tool_strip_drop_down_background; } }
		public virtual Color_ ToolStripGradientBegin { get { return this.tool_strip_gradient_begin; } }
		public virtual Color_ ToolStripGradientEnd { get { return this.tool_strip_gradient_end; } }
		public virtual Color_ ToolStripGradientMiddle { get { return this.tool_strip_gradient_middle; } }
		public virtual Color_ ToolStripPanelGradientBegin { get { return this.tool_strip_panel_gradient_begin; } }
		public virtual Color_ ToolStripPanelGradientEnd { get { return this.tool_strip_panel_gradient_end; } }
		public bool UseSystemColors {
			get { return use_system_colors; }
			set {
				if (value != use_system_colors) {
					use_system_colors = value; CalculateColors ();
				}
			}
		}
        #endregion

        #region Private Methods
        private void CalculateColors ()
		{
			switch (GetCurrentStyle ()) {
				case ColorSchemes.Classic:
					button_checked_gradient_begin = Color_.Empty;
					button_checked_gradient_end = Color_.Empty;
					button_checked_gradient_middle = Color_.Empty;
					button_checked_highlight = Color_.FromArgb (184, 191, 211);

                    button_checked_highlight_border = Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5);

                    button_pressed_border = Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5);
                    button_pressed_gradient_begin = Color_.FromArgb (133, 146, 181);
					button_pressed_gradient_end = Color_.FromArgb (133, 146, 181);
					button_pressed_gradient_middle = Color_.FromArgb (133, 146, 181);
					button_pressed_highlight = Color_.FromArgb (131, 144, 179);
					button_pressed_highlight_border = Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5);

                    button_selected_border = Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5);
                    button_selected_gradient_begin = Color_.FromArgb (182, 189, 210);
					button_selected_gradient_end = Color_.FromArgb (182, 189, 210);
					button_selected_gradient_middle = Color_.FromArgb (182, 189, 210);
					button_selected_highlight = Color_.FromArgb (184, 191, 211);
					button_selected_highlight_border = Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5);

                    check_background = Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5);
                    check_pressed_background = Color_.FromArgb (133, 146, 181);
					check_selected_background = Color_.FromArgb (133, 146, 181);

					grip_dark = Color_.FromArgb (160, 160, 160);
					grip_light = SystemColors.Window;

					image_margin_gradient_begin = Color_.FromArgb (245, 244, 242);
					image_margin_gradient_end = SystemColors.Control;
					image_margin_gradient_middle = Color_.FromArgb (234, 232, 228);
					image_margin_revealed_gradient_begin = Color_.FromArgb (238, 236, 233);
					image_margin_revealed_gradient_end = Color_.FromArgb (216, 213, 206);
					image_margin_revealed_gradient_middle = Color_.FromArgb (225, 222, 217);

					menu_border = Color_.FromArgb (102, 102, 102);
					menu_item_border = SystemColors.Highlight;

					menu_item_pressed_gradient_begin = Color_.FromArgb (245, 244, 242);
					menu_item_pressed_gradient_end = Color_.FromArgb (234, 232, 228);
					menu_item_pressed_gradient_middle = Color_.FromArgb (225, 222, 217);
					menu_item_selected = SystemColors.Window;
					menu_item_selected_gradient_begin = Color_.FromArgb (182, 189, 210);
					menu_item_selected_gradient_end = Color_.FromArgb (182, 189, 210);

					menu_strip_gradient_begin = SystemColors.ButtonFace;
					menu_strip_gradient_end = Color_.FromArgb (246, 245, 244);

					overflow_button_gradient_begin = Color_.FromArgb (225, 222, 217);
					overflow_button_gradient_end = SystemColors.ButtonShadow;
					overflow_button_gradient_middle = Color_.FromArgb (216, 213, 206);

					rafting_container_gradient_begin = SystemColors.ButtonFace;
					rafting_container_gradient_end = Color_.FromArgb (246, 245, 244);

					separator_dark = Color_.FromArgb (166, 166, 166);
					separator_light = SystemColors.ButtonHighlight;

					status_strip_gradient_begin = SystemColors.ButtonFace;
					status_strip_gradient_end = Color_.FromArgb (246, 245, 244);

					tool_strip_border = Color_.FromArgb (219, 216, 209);
					tool_strip_content_panel_gradient_begin = SystemColors.ButtonFace;
					tool_strip_content_panel_gradient_end = Color_.FromArgb (246, 245, 244);
					tool_strip_drop_down_background = SystemColors.Window;

					tool_strip_gradient_begin = Color_.FromArgb (245, 244, 242);
					tool_strip_gradient_end = SystemColors.ButtonFace;
					tool_strip_gradient_middle = Color_.FromArgb (234, 232, 228);

					tool_strip_panel_gradient_begin = SystemColors.ButtonFace;
					tool_strip_panel_gradient_end = Color_.FromArgb (246, 245, 244);
					break;
				case ColorSchemes.NormalColor:
					button_checked_gradient_begin = use_system_colors ? Color_.Empty : Color_.FromArgb (255, 223, 154);
					button_checked_gradient_end = use_system_colors ? Color_.Empty : Color_.FromArgb (255, 166, 76);
					button_checked_gradient_middle = use_system_colors ? Color_.Empty : Color_.FromArgb (255, 195, 116);
					button_checked_highlight = Color_.FromArgb (195, 211, 237);
					button_checked_highlight_border = Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5);
                    button_pressed_border = use_system_colors ? Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5) : Color_.FromArgb (0, 0, 128);
					button_pressed_gradient_begin = use_system_colors ? Color_.FromArgb (152, 181, 226) : Color_.FromArgb (254, 128, 62);
					button_pressed_gradient_end = use_system_colors ? Color_.FromArgb (152, 181, 226) : Color_.FromArgb (255, 223, 154);
					button_pressed_gradient_middle = use_system_colors ? Color_.FromArgb (152, 181, 226) : Color_.FromArgb (255, 177, 109);
					button_pressed_highlight = use_system_colors ? Color_.FromArgb (150, 179, 225) : Color_.FromArgb (150, 179, 225);
					button_pressed_highlight_border = Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5); 
					button_selected_border = use_system_colors ? Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5) : Color_.FromArgb (0, 0, 128);
					button_selected_gradient_begin = use_system_colors ? Color_.FromArgb (193, 210, 238) : Color_.FromArgb (255, 255, 222);
					button_selected_gradient_end = use_system_colors ? Color_.FromArgb (193, 210, 238) : Color_.FromArgb (255, 203, 136);
					button_selected_gradient_middle = use_system_colors ? Color_.FromArgb (193, 210, 238) : Color_.FromArgb (255, 225, 172);
					button_selected_highlight = use_system_colors ? Color_.FromArgb (195, 211, 237) : Color_.FromArgb (195, 211, 237);
					button_selected_highlight_border = use_system_colors ? Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5) : Color_.FromArgb (0, 0, 128);

					check_background = use_system_colors ? Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5) : Color_.FromArgb (255, 192, 111);
					check_pressed_background = use_system_colors ? Color_.FromArgb (152, 181, 226) : Color_.FromArgb (254, 128, 62);
					check_selected_background = use_system_colors ? Color_.FromArgb (152, 181, 226) : Color_.FromArgb (254, 128, 62);

					grip_dark = use_system_colors ? Color_.FromArgb (193, 190, 179) : Color_.FromArgb (39, 65, 118);
					grip_light = use_system_colors ? SystemColors.Window : Color_.FromArgb (255, 255, 255);

					image_margin_gradient_begin = use_system_colors ? Color_.FromArgb (251, 250, 246) : Color_.FromArgb (227, 239, 255);
					image_margin_gradient_end = use_system_colors ? SystemColors.Control : Color_.FromArgb (123, 164, 224);
					image_margin_gradient_middle = use_system_colors ? Color_.FromArgb (246, 244, 236) : Color_.FromArgb (203, 225, 252);
					image_margin_revealed_gradient_begin = use_system_colors ? Color_.FromArgb (247, 246, 239) : Color_.FromArgb (203, 221, 246);
					image_margin_revealed_gradient_end = use_system_colors ? Color_.FromArgb (238, 235, 220) : Color_.FromArgb (114, 155, 215);
					image_margin_revealed_gradient_middle = use_system_colors ? Color_.FromArgb (242, 240, 228) : Color_.FromArgb (161, 197, 249);

					menu_border = use_system_colors ? Color_.FromArgb (138, 134, 122) : Color_.FromArgb (0, 45, 150);
					menu_item_border = use_system_colors ? SystemColors.Highlight : Color_.FromArgb (0, 0, 128);

					menu_item_pressed_gradient_begin = use_system_colors ? Color_.FromArgb (251, 250, 246) : Color_.FromArgb (227, 239, 255);
					menu_item_pressed_gradient_end = use_system_colors ? Color_.FromArgb (246, 244, 236) : Color_.FromArgb (123, 164, 224);
					menu_item_pressed_gradient_middle = use_system_colors ? Color_.FromArgb (242, 240, 228) : Color_.FromArgb (161, 197, 249);
					menu_item_selected = use_system_colors ? SystemColors.Window : Color_.FromArgb (255, 238, 194);
					menu_item_selected_gradient_begin = use_system_colors ? Color_.FromArgb (193, 210, 238) : Color_.FromArgb (255, 255, 222);
					menu_item_selected_gradient_end = use_system_colors ? Color_.FromArgb (193, 210, 238) : Color_.FromArgb (255, 203, 136);

					menu_strip_gradient_begin = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (158, 190, 245);
					menu_strip_gradient_end = use_system_colors ? Color_.FromArgb (251, 250, 247) : Color_.FromArgb (196, 218, 250);

					overflow_button_gradient_begin = use_system_colors ? Color_.FromArgb (242, 240, 228) : Color_.FromArgb (127, 177, 250);
					overflow_button_gradient_end = use_system_colors ? SystemColors.ButtonShadow : Color_.FromArgb (0, 53, 145);
					overflow_button_gradient_middle = use_system_colors ? Color_.FromArgb (238, 235, 220) : Color_.FromArgb (82, 127, 208);

					rafting_container_gradient_begin = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (158, 190, 245);
					rafting_container_gradient_end = use_system_colors ? Color_.FromArgb (251, 250, 247) : Color_.FromArgb (196, 218, 250);

					separator_dark = use_system_colors ? Color_.FromArgb (197, 194, 184) : Color_.FromArgb (106, 140, 203);
					separator_light = use_system_colors ? SystemColors.ButtonHighlight : Color_.FromArgb (241, 249, 255);

					status_strip_gradient_begin = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (158, 190, 245);
					status_strip_gradient_end = use_system_colors ? Color_.FromArgb (251, 250, 247) : Color_.FromArgb (196, 218, 250);

					tool_strip_border = use_system_colors ? Color_.FromArgb (239, 237, 222) : Color_.FromArgb (59, 97, 156);
					tool_strip_content_panel_gradient_begin = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (158, 190, 245);
					tool_strip_content_panel_gradient_end = use_system_colors ? Color_.FromArgb (251, 250, 247) : Color_.FromArgb (196, 218, 250);
					tool_strip_drop_down_background = use_system_colors ? Color_.FromArgb (252, 252, 249) : Color_.FromArgb (246, 246, 246);

					tool_strip_gradient_begin = use_system_colors ? Color_.FromArgb (251, 250, 246) : Color_.FromArgb (227, 239, 255);
					tool_strip_gradient_end = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (123, 164, 224);
					tool_strip_gradient_middle = use_system_colors ? Color_.FromArgb (246, 244, 236) : Color_.FromArgb (203, 225, 252);

					tool_strip_panel_gradient_begin = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (158, 190, 245);
					tool_strip_panel_gradient_end = use_system_colors ? Color_.FromArgb (251, 250, 247) : Color_.FromArgb (196, 218, 250);
					break;
				case ColorSchemes.HomeStead:
					button_checked_gradient_begin = use_system_colors ? Color_.Empty : Color_.FromArgb (255, 223, 154);
					button_checked_gradient_end = use_system_colors ? Color_.Empty : Color_.FromArgb (255, 166, 76);
					button_checked_gradient_middle = use_system_colors ? Color_.Empty : Color_.FromArgb (255, 195, 116);
					button_checked_highlight = Color_.FromArgb (223, 227, 213);
					button_checked_highlight_border = Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5);

					button_pressed_border = use_system_colors ? Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5) : Color_.FromArgb (63, 93, 56);
					button_pressed_gradient_begin = use_system_colors ? Color_.FromArgb (201, 208, 184) : Color_.FromArgb (254, 128, 62);
					button_pressed_gradient_end = use_system_colors ? Color_.FromArgb (201, 208, 184) : Color_.FromArgb (255, 223, 154);
					button_pressed_gradient_middle = use_system_colors ? Color_.FromArgb (201, 208, 184) : Color_.FromArgb (255, 177, 109);
					button_pressed_highlight = use_system_colors ? Color_.FromArgb (200, 206, 182) : Color_.FromArgb (200, 206, 182);
					button_pressed_highlight_border = Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5);

					button_selected_border = use_system_colors ? Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5) : Color_.FromArgb (63, 93, 56);
					button_selected_gradient_begin = use_system_colors ? Color_.FromArgb (223, 227, 212) : Color_.FromArgb (255, 255, 222);
					button_selected_gradient_end = use_system_colors ? Color_.FromArgb (223, 227, 212) : Color_.FromArgb (255, 203, 136);
					button_selected_gradient_middle = use_system_colors ? Color_.FromArgb (223, 227, 212) : Color_.FromArgb (255, 225, 172);
					button_selected_highlight = use_system_colors ? Color_.FromArgb (223, 227, 213) : Color_.FromArgb (223, 227, 213);
					button_selected_highlight_border = use_system_colors ? Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5) : Color_.FromArgb (63, 93, 56);

					check_background = use_system_colors ? Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5) : Color_.FromArgb (255, 192, 111);
					check_pressed_background = use_system_colors ? Color_.FromArgb (201, 208, 184) : Color_.FromArgb (254, 128, 62);
					check_selected_background = use_system_colors ? Color_.FromArgb (201, 208, 184) : Color_.FromArgb (254, 128, 62);

					grip_dark = use_system_colors ? Color_.FromArgb (193, 190, 179) : Color_.FromArgb (81, 94, 51);
					grip_light = use_system_colors ? SystemColors.Window : Color_.FromArgb (255, 255, 255);

					image_margin_gradient_begin = use_system_colors ? Color_.FromArgb (251, 250, 246) : Color_.FromArgb (255, 255, 237);
					image_margin_gradient_end = use_system_colors ? SystemColors.Control : Color_.FromArgb (181, 196, 143);
					image_margin_gradient_middle = use_system_colors ? Color_.FromArgb (246, 244, 236) : Color_.FromArgb (206, 220, 167);
					image_margin_revealed_gradient_begin = use_system_colors ? Color_.FromArgb (247, 246, 239) : Color_.FromArgb (230, 230, 209);
					image_margin_revealed_gradient_end = use_system_colors ? Color_.FromArgb (238, 235, 220) : Color_.FromArgb (160, 177, 116);
					image_margin_revealed_gradient_middle = use_system_colors ? Color_.FromArgb (242, 240, 228) : Color_.FromArgb (186, 201, 143);

					menu_border = use_system_colors ? Color_.FromArgb (138, 134, 122) : Color_.FromArgb (117, 141, 94);
					menu_item_border = use_system_colors ? SystemColors.Highlight : Color_.FromArgb (63, 93, 56);

					menu_item_pressed_gradient_begin = use_system_colors ? Color_.FromArgb (251, 250, 246) : Color_.FromArgb (237, 240, 214);
					menu_item_pressed_gradient_end = use_system_colors ? Color_.FromArgb (246, 244, 236) : Color_.FromArgb (181, 196, 143);
					menu_item_pressed_gradient_middle = use_system_colors ? Color_.FromArgb (242, 240, 228) : Color_.FromArgb (186, 201, 143);
					menu_item_selected = use_system_colors ? SystemColors.Window : Color_.FromArgb (255, 238, 194);
					menu_item_selected_gradient_begin = use_system_colors ? Color_.FromArgb (223, 227, 212) : Color_.FromArgb (255, 255, 222);
					menu_item_selected_gradient_end = use_system_colors ? Color_.FromArgb (223, 227, 212) : Color_.FromArgb (255, 203, 136);

					menu_strip_gradient_begin = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (217, 217, 167);
					menu_strip_gradient_end = use_system_colors ? Color_.FromArgb (251, 250, 247) : Color_.FromArgb (242, 241, 228);

					overflow_button_gradient_begin = use_system_colors ? Color_.FromArgb (242, 240, 228) : Color_.FromArgb (186, 204, 150);
					overflow_button_gradient_end = use_system_colors ? SystemColors.ButtonShadow : Color_.FromArgb (96, 119, 107);
					overflow_button_gradient_middle = use_system_colors ? Color_.FromArgb (238, 235, 220) : Color_.FromArgb (141, 160, 107);

					rafting_container_gradient_begin = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (217, 217, 167);
					rafting_container_gradient_end = use_system_colors ? Color_.FromArgb (251, 250, 247) : Color_.FromArgb (242, 241, 228);

					separator_dark = use_system_colors ? Color_.FromArgb (197, 194, 184) : Color_.FromArgb (96, 128, 88);
					separator_light = use_system_colors ? SystemColors.ButtonHighlight : Color_.FromArgb (244, 247, 222);

					status_strip_gradient_begin = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (217, 217, 167);
					status_strip_gradient_end = use_system_colors ? Color_.FromArgb (251, 250, 247) : Color_.FromArgb (242, 241, 228);

					tool_strip_border = use_system_colors ? Color_.FromArgb (239, 237, 222) : Color_.FromArgb (96, 128, 88);
					tool_strip_content_panel_gradient_begin = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (217, 217, 167);
					tool_strip_content_panel_gradient_end = use_system_colors ? Color_.FromArgb (251, 250, 247) : Color_.FromArgb (242, 241, 228);
					tool_strip_drop_down_background = use_system_colors ? Color_.FromArgb (252, 252, 249) : Color_.FromArgb (244, 244, 238);

					tool_strip_gradient_begin = use_system_colors ? Color_.FromArgb (251, 250, 246) : Color_.FromArgb (255, 255, 237);
					tool_strip_gradient_end = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (181, 196, 143);
					tool_strip_gradient_middle = use_system_colors ? Color_.FromArgb (246, 244, 236) : Color_.FromArgb (206, 220, 167);

					tool_strip_panel_gradient_begin = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (217, 217, 167);
					tool_strip_panel_gradient_end = use_system_colors ? Color_.FromArgb (251, 250, 247) : Color_.FromArgb (242, 241, 228);
					break;
				case ColorSchemes.Metallic:
					button_checked_gradient_begin = use_system_colors ? Color_.Empty : Color_.FromArgb (255, 223, 154);
					button_checked_gradient_end = use_system_colors ? Color_.Empty : Color_.FromArgb (255, 166, 76);
					button_checked_gradient_middle = use_system_colors ? Color_.Empty : Color_.FromArgb (255, 195, 116);
					button_checked_highlight = Color_.FromArgb (231, 232, 235);
					button_checked_highlight_border = Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5);

					button_pressed_border = use_system_colors ? Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5) : Color_.FromArgb (75, 75, 111);
					button_pressed_gradient_begin = use_system_colors ? Color_.FromArgb (217, 218, 223) : Color_.FromArgb (254, 128, 62);
					button_pressed_gradient_end = use_system_colors ? Color_.FromArgb (217, 218, 223) : Color_.FromArgb (255, 223, 154);
					button_pressed_gradient_middle = use_system_colors ? Color_.FromArgb (217, 218, 223) : Color_.FromArgb (255, 177, 109);
					button_pressed_highlight = use_system_colors ? Color_.FromArgb (215, 216, 222) : Color_.FromArgb (215, 216, 222);
					button_pressed_highlight_border = Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5);

					button_selected_border = use_system_colors ? Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5) : Color_.FromArgb (75, 75, 111);
					button_selected_gradient_begin = use_system_colors ? Color_.FromArgb (232, 233, 236) : Color_.FromArgb (255, 255, 222);
					button_selected_gradient_end = use_system_colors ? Color_.FromArgb (232, 233, 236) : Color_.FromArgb (255, 203, 136);
					button_selected_gradient_middle = use_system_colors ? Color_.FromArgb (232, 233, 236) : Color_.FromArgb (255, 225, 172);
					button_selected_highlight = use_system_colors ? Color_.FromArgb (231, 232, 235) : Color_.FromArgb (231, 232, 235);
					button_selected_highlight_border = use_system_colors ? Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5) : Color_.FromArgb (75, 75, 111);

					check_background = use_system_colors ? Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5) : Color_.FromArgb (255, 192, 111);
					check_pressed_background = use_system_colors ? Color_.FromArgb (217, 218, 223) : Color_.FromArgb (254, 128, 62);
					check_selected_background = use_system_colors ? Color_.FromArgb (217, 218, 223) : Color_.FromArgb (254, 128, 62);

					grip_dark = use_system_colors ? Color_.FromArgb (182, 182, 185) : Color_.FromArgb (84, 84, 117);
					grip_light = use_system_colors ? SystemColors.Window : Color_.FromArgb (255, 255, 255);

					image_margin_gradient_begin = use_system_colors ? Color_.FromArgb (248, 248, 249) : Color_.FromArgb (249, 249, 255);
					image_margin_gradient_end = use_system_colors ? SystemColors.Control : Color_.FromArgb (147, 145, 176);
					image_margin_gradient_middle = use_system_colors ? Color_.FromArgb (240, 239, 241) : Color_.FromArgb (225, 226, 236);
					image_margin_revealed_gradient_begin = use_system_colors ? Color_.FromArgb (243, 242, 244) : Color_.FromArgb (215, 215, 226);
					image_margin_revealed_gradient_end = use_system_colors ? Color_.FromArgb (227, 226, 230) : Color_.FromArgb (118, 116, 151);
					image_margin_revealed_gradient_middle = use_system_colors ? Color_.FromArgb (233, 233, 235) : Color_.FromArgb (184, 185, 202);

					menu_border = use_system_colors ? Color_.FromArgb (126, 126, 129) : Color_.FromArgb (124, 124, 148);
					menu_item_border = use_system_colors ? SystemColors.Highlight : Color_.FromArgb (75, 75, 111);

					menu_item_pressed_gradient_begin = use_system_colors ? Color_.FromArgb (248, 248, 249) : Color_.FromArgb (232, 233, 242);
					menu_item_pressed_gradient_end = use_system_colors ? Color_.FromArgb (240, 239, 241) : Color_.FromArgb (172, 170, 194);
					menu_item_pressed_gradient_middle = use_system_colors ? Color_.FromArgb (233, 233, 235) : Color_.FromArgb (184, 185, 202);
					menu_item_selected = use_system_colors ? SystemColors.Window : Color_.FromArgb (255, 238, 194);
					menu_item_selected_gradient_begin = use_system_colors ? Color_.FromArgb (232, 233, 236) : Color_.FromArgb (255, 255, 222);
					menu_item_selected_gradient_end = use_system_colors ? Color_.FromArgb (232, 233, 236) : Color_.FromArgb (255, 203, 136);

					menu_strip_gradient_begin = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (215, 215, 229);
					menu_strip_gradient_end = use_system_colors ? Color_.FromArgb (249, 248, 249) : Color_.FromArgb (243, 243, 247);

					overflow_button_gradient_begin = use_system_colors ? Color_.FromArgb (233, 233, 235) : Color_.FromArgb (186, 185, 206);
					overflow_button_gradient_end = use_system_colors ? SystemColors.ButtonShadow : Color_.FromArgb (118, 116, 146);
					overflow_button_gradient_middle = use_system_colors ? Color_.FromArgb (227, 226, 230) : Color_.FromArgb (156, 155, 180);

					rafting_container_gradient_begin = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (215, 215, 229);
					rafting_container_gradient_end = use_system_colors ? Color_.FromArgb (249, 248, 249) : Color_.FromArgb (243, 243, 247);

					separator_dark = use_system_colors ? Color_.FromArgb (186, 186, 189) : Color_.FromArgb (110, 109, 143);
					separator_light = use_system_colors ? SystemColors.ButtonHighlight : Color_.FromArgb (255, 255, 255);

					status_strip_gradient_begin = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (215, 215, 229);
					status_strip_gradient_end = use_system_colors ? Color_.FromArgb (249, 248, 249) : Color_.FromArgb (243, 243, 247);

					tool_strip_border = use_system_colors ? Color_.FromArgb (229, 228, 232) : Color_.FromArgb (124, 124, 148);
					tool_strip_content_panel_gradient_begin = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (215, 215, 229);
					tool_strip_content_panel_gradient_end = use_system_colors ? Color_.FromArgb (249, 248, 249) : Color_.FromArgb (243, 243, 247);
					tool_strip_drop_down_background = use_system_colors ? Color_.FromArgb (251, 250, 251) : Color_.FromArgb (253, 250, 255);

					tool_strip_gradient_begin = use_system_colors ? Color_.FromArgb (248, 248, 249) : Color_.FromArgb (249, 249, 255);
					tool_strip_gradient_end = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (147, 145, 176);
					tool_strip_gradient_middle = use_system_colors ? Color_.FromArgb (240, 239, 241) : Color_.FromArgb (225, 226, 236);

					tool_strip_panel_gradient_begin = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (215, 215, 229);
					tool_strip_panel_gradient_end = use_system_colors ? Color_.FromArgb (249, 248, 249) : Color_.FromArgb (243, 243, 247);
					break;
				case ColorSchemes.MediaCenter:
					button_checked_gradient_begin = use_system_colors ? Color_.Empty : Color_.FromArgb (226, 229, 238);
					button_checked_gradient_end = use_system_colors ? Color_.Empty : Color_.FromArgb (226, 229, 238);
					button_checked_gradient_middle = use_system_colors ? Color_.Empty : Color_.FromArgb (226, 229, 238);
					button_checked_highlight = Color_.FromArgb (196, 208, 229);
					button_checked_highlight_border = Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5);

					button_pressed_border = use_system_colors ? Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5) : Color_.FromArgb (51, 94, 168);
					button_pressed_gradient_begin = use_system_colors ? Color_.FromArgb (153, 175, 212) : Color_.FromArgb (153, 175, 212);
					button_pressed_gradient_end = use_system_colors ? Color_.FromArgb (153, 175, 212) : Color_.FromArgb (153, 175, 212);
					button_pressed_gradient_middle = use_system_colors ? Color_.FromArgb (153, 175, 212) : Color_.FromArgb (153, 175, 212);
					button_pressed_highlight = use_system_colors ? Color_.FromArgb (152, 173, 210) : Color_.FromArgb (152, 173, 210);
					button_pressed_highlight_border = Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5);

					button_selected_border = use_system_colors ? Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5) : Color_.FromArgb (51, 94, 168);
					button_selected_gradient_begin = use_system_colors ? Color_.FromArgb (194, 207, 229) : Color_.FromArgb (194, 207, 229);
					button_selected_gradient_end = use_system_colors ? Color_.FromArgb (194, 207, 229) : Color_.FromArgb (194, 207, 229);
					button_selected_gradient_middle = use_system_colors ? Color_.FromArgb (194, 207, 229) : Color_.FromArgb (194, 207, 229);
					button_selected_highlight = use_system_colors ? Color_.FromArgb (196, 208, 229) : Color_.FromArgb (196, 208, 229);
					button_selected_highlight_border = use_system_colors ? Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5) : Color_.FromArgb (51, 94, 168);

					check_background = use_system_colors ? Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5) : Color_.FromArgb (226, 229, 238);
					check_pressed_background = use_system_colors ? Color_.FromArgb (153, 175, 212) : Color_.FromArgb (51, 94, 168);
					check_selected_background = use_system_colors ? Color_.FromArgb (153, 175, 212) : Color_.FromArgb (51, 94, 168);

					grip_dark = use_system_colors ? Color_.FromArgb (189, 188, 191) : Color_.FromArgb (189, 188, 191);
					grip_light = use_system_colors ? SystemColors.Window : Color_.FromArgb (255, 255, 255);

					image_margin_gradient_begin = use_system_colors ? Color_.FromArgb (250, 250, 251) : Color_.FromArgb (252, 252, 252);
					image_margin_gradient_end = use_system_colors ? SystemColors.Control : Color_.FromArgb (235, 233, 237);
					image_margin_gradient_middle = use_system_colors ? Color_.FromArgb (245, 244, 246) : Color_.FromArgb (245, 244, 246);
					image_margin_revealed_gradient_begin = use_system_colors ? Color_.FromArgb (247, 246, 248) : Color_.FromArgb (247, 246, 248);
					image_margin_revealed_gradient_end = use_system_colors ? Color_.FromArgb (237, 235, 239) : Color_.FromArgb (228, 226, 230);
					image_margin_revealed_gradient_middle = use_system_colors ? Color_.FromArgb (241, 240, 242) : Color_.FromArgb (241, 240, 242);

					menu_border = use_system_colors ? Color_.FromArgb (134, 133, 136) : Color_.FromArgb (134, 133, 136);
					menu_item_border = use_system_colors ? SystemColors.Highlight : Color_.FromArgb (51, 94, 168);

					menu_item_pressed_gradient_begin = use_system_colors ? Color_.FromArgb (250, 250, 251) : Color_.FromArgb (252, 252, 252);
					menu_item_pressed_gradient_end = use_system_colors ? Color_.FromArgb (245, 244, 246) : Color_.FromArgb (245, 244, 246);
					menu_item_pressed_gradient_middle = use_system_colors ? Color_.FromArgb (241, 240, 242) : Color_.FromArgb (241, 240, 242);
					menu_item_selected = use_system_colors ? SystemColors.Window : Color_.FromArgb (194, 207, 229);
					menu_item_selected_gradient_begin = use_system_colors ? Color_.FromArgb (194, 207, 229) : Color_.FromArgb (194, 207, 229);
					menu_item_selected_gradient_end = use_system_colors ? Color_.FromArgb (194, 207, 229) : Color_.FromArgb (194, 207, 229);

					menu_strip_gradient_begin = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (235, 233, 237);
					menu_strip_gradient_end = use_system_colors ? Color_.FromArgb (251, 250, 251) : Color_.FromArgb (251, 250, 251);

					overflow_button_gradient_begin = use_system_colors ? Color_.FromArgb (241, 240, 242) : Color_.FromArgb (242, 242, 242);
					overflow_button_gradient_end = use_system_colors ? SystemColors.ButtonShadow : Color_.FromArgb (167, 166, 170);
					overflow_button_gradient_middle = use_system_colors ? Color_.FromArgb (237, 235, 239) : Color_.FromArgb (224, 224, 225);

					rafting_container_gradient_begin = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (235, 233, 237);
					rafting_container_gradient_end = use_system_colors ? Color_.FromArgb (251, 250, 251) : Color_.FromArgb (251, 250, 251);

					separator_dark = use_system_colors ? Color_.FromArgb (193, 193, 196) : Color_.FromArgb (193, 193, 196);
					separator_light = use_system_colors ? SystemColors.ButtonHighlight : Color_.FromArgb (255, 255, 255);

					status_strip_gradient_begin = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (235, 233, 237);
					status_strip_gradient_end = use_system_colors ? Color_.FromArgb (251, 250, 251) : Color_.FromArgb (251, 250, 251);

					tool_strip_border = use_system_colors ? Color_.FromArgb (238, 237, 240) : Color_.FromArgb (238, 237, 240);
					tool_strip_content_panel_gradient_begin = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (235, 233, 237);
					tool_strip_content_panel_gradient_end = use_system_colors ? Color_.FromArgb (251, 250, 251) : Color_.FromArgb (251, 250, 251);
					tool_strip_drop_down_background = use_system_colors ? Color_.FromArgb (252, 252, 252) : Color_.FromArgb (252, 252, 252);

					tool_strip_gradient_begin = use_system_colors ? Color_.FromArgb (250, 250, 251) : Color_.FromArgb (252, 252, 252);
					tool_strip_gradient_end = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (235, 233, 237);
					tool_strip_gradient_middle = use_system_colors ? Color_.FromArgb (245, 244, 246) : Color_.FromArgb (245, 244, 246);

					tool_strip_panel_gradient_begin = use_system_colors ? SystemColors.ButtonFace : Color_.FromArgb (235, 233, 237);
					tool_strip_panel_gradient_end = use_system_colors ? Color_.FromArgb (251, 250, 251) : Color_.FromArgb (251, 250, 251);
					break;
				case ColorSchemes.Aero:
					button_checked_gradient_begin = Color_.Empty;
					button_checked_gradient_end = Color_.Empty;
					button_checked_gradient_middle = Color_.Empty;
					button_checked_highlight = Color_.FromArgb (196, 225, 255);
					button_checked_highlight_border = Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5);

					button_pressed_border = Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5);
					button_pressed_gradient_begin = Color_.FromArgb (153, 204, 255);
					button_pressed_gradient_end = Color_.FromArgb (153, 204, 255);
					button_pressed_gradient_middle = Color_.FromArgb (153, 204, 255);
					button_pressed_highlight = Color_.FromArgb (152, 203, 255);
					button_pressed_highlight_border = Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5);

					button_selected_border = use_system_colors ? Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5) : Color_.FromArgb (51, 94, 168);
					button_selected_gradient_begin = Color_.FromArgb (194, 224, 255);
					button_selected_gradient_end = Color_.FromArgb (194, 224, 255);
					button_selected_gradient_middle = Color_.FromArgb (194, 224, 255);
					button_selected_highlight = Color_.FromArgb (196, 225, 255);
					button_selected_highlight_border = Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5);

					check_background = Color_.FromArgb(0xFF, 0x31, 0x6A, 0xC5);
					check_pressed_background = Color_.FromArgb (153, 204, 255);
					check_selected_background = Color_.FromArgb (153, 204, 255);

					grip_dark = Color_.FromArgb (184, 184, 184);
					grip_light = SystemColors.Window;

					image_margin_gradient_begin = Color_.FromArgb (252, 252, 252);
					image_margin_gradient_end = SystemColors.Control;
					image_margin_gradient_middle = Color_.FromArgb (250, 250, 250);
					image_margin_revealed_gradient_begin = Color_.FromArgb (251, 251, 251);
					image_margin_revealed_gradient_end = Color_.FromArgb (245, 245, 245);
					image_margin_revealed_gradient_middle = Color_.FromArgb (247, 247, 247);

					menu_border = Color_.FromArgb (128, 128, 128);
					menu_item_border = SystemColors.Highlight;

					menu_item_pressed_gradient_begin = Color_.FromArgb (252, 252, 252);
					menu_item_pressed_gradient_end = Color_.FromArgb (250, 250, 250);
					menu_item_pressed_gradient_middle = Color_.FromArgb (247, 247, 247);
					menu_item_selected = SystemColors.Window;
					menu_item_selected_gradient_begin = Color_.FromArgb (194, 224, 255);
					menu_item_selected_gradient_end = Color_.FromArgb (194, 224, 255);

					menu_strip_gradient_begin = SystemColors.ButtonFace;
					menu_strip_gradient_end = Color_.FromArgb (253, 253, 253);

					overflow_button_gradient_begin = Color_.FromArgb (247, 247, 247);
					overflow_button_gradient_end = SystemColors.ButtonShadow;
					overflow_button_gradient_middle = Color_.FromArgb (245, 245, 245);

					rafting_container_gradient_begin = SystemColors.ButtonFace;
					rafting_container_gradient_end = Color_.FromArgb (253, 253, 253);

					separator_dark = Color_.FromArgb (189, 189, 189);
					separator_light = SystemColors.ButtonHighlight;

					status_strip_gradient_begin = SystemColors.ButtonFace;
					status_strip_gradient_end = Color_.FromArgb (253, 253, 253);

					tool_strip_border = Color_.FromArgb (246, 246, 246);
					tool_strip_content_panel_gradient_begin = SystemColors.ButtonFace;
					tool_strip_content_panel_gradient_end = Color_.FromArgb (253, 253, 253);
					tool_strip_drop_down_background = Color_.FromArgb (253, 253, 253);

					tool_strip_gradient_begin = Color_.FromArgb (252, 252, 252);
					tool_strip_gradient_end = SystemColors.ButtonFace;
					tool_strip_gradient_middle = Color_.FromArgb (250, 250, 250);

					tool_strip_panel_gradient_begin = SystemColors.ButtonFace;
					tool_strip_panel_gradient_end = Color_.FromArgb (253, 253, 253);
					break;
			}
		}
		
		private ColorSchemes GetCurrentStyle ()
		{
			if (!VisualStyleInformation.IsEnabledByUser || string.IsNullOrEmpty (VisualStylesEngine.Instance.VisualStyleInformationFileName))


				return ColorSchemes.Classic;
			else {
				switch (System.IO.Path.GetFileNameWithoutExtension (VisualStylesEngine.Instance.VisualStyleInformationFileName).ToLowerInvariant ()) {
					case "aero":
						return ColorSchemes.Aero;
					case "royale":
						return ColorSchemes.MediaCenter;
					default:
						switch (VisualStyleInformation.ColorScheme) {
							case "NormalColor":
								return ColorSchemes.NormalColor;
							case "HomeStead":
								return ColorSchemes.HomeStead;
							case "Metallic":
								return ColorSchemes.Metallic;
							default:
								return ColorSchemes.Classic;
						}
				}
			}
		}
		#endregion

		#region Private Enums
		private enum ColorSchemes
		{
			Classic,	// Windows Classic (No theme)
			NormalColor,	// Luna Blue
			HomeStead,	// Luna Olive
			Metallic,	// Luna Silver
			MediaCenter,	// Media Center (Energy Blue)
			Aero		// Windows Vista
		}
		#endregion
	}
}
