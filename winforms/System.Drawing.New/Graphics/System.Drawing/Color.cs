//
// System.Drawing.Color.cs
//
// Authors:
// 	Dennis Hayes (dennish@raytek.com)
// 	Ben Houston  (ben@exocortex.org)
// 	Gonzalo Paniagua (gonzalo@ximian.com)
// 	Juraj Skripsky (juraj@hotfeet.ch)
//	Sebastien Pouliot  <sebastien@ximian.com>
//
// (C) 2002 Dennis Hayes
// (c) 2002 Ximian, Inc. (http://www.ximiam.com)
// (C) 2005 HotFeet GmbH (http://www.hotfeet.ch)
// Copyright (C) 2004,2006-2007 Novell, Inc (http://www.novell.com)
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

using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Drawing 
{
	[TypeConverter(typeof(ColorConverter))]
#if ONLY_1_1
	[ComVisible (true)]
#endif
	[Editor ("System.Drawing.Design.ColorEditor, " + Consts.AssemblySystem_Drawing_Design, typeof (System.Drawing.Design.UITypeEditor))]
	[Serializable]
	public struct Color_ {

		// Private transparency (A) and R,G,B fields.
		private long value;

		// The specs also indicate that all three of these properties are true
		// if created with FromKnownColor or FromNamedColor, false otherwise (FromARGB).
		// Per Microsoft and ECMA specs these varibles are set by which constructor is used, not by their values.
		[Flags]
		internal enum ColorType : short {
			Empty=0,
			Known=1,
			ARGB=2,
			Named=4,
			System=8
		}

		internal short state;
		internal short knownColor;
// #if ONLY_1_1
// Mono bug #324144 is holding this change
		// MS 1.1 requires this member to be present for serialization (not so in 2.0)
		// however it's bad to keep a string (reference) in a struct
		internal string name;
// #endif

        public static implicit operator Color(Color_ cr)
        {
            return Color.FromArgb(cr.A, cr.R, cr.G, cr.B);
        }

        public static implicit operator Color_(Color cr)
        {
            return Color_.FromArgb(cr.A, cr.R, cr.G, cr.B);
        }

        public string Name {
			get {
#if NET_2_0_ONCE_MONO_BUG_324144_IS_FIXED
				if (IsNamedColor)
					return KnownColors.GetName (knownColor);
				else
					return String.Format ("{0:x}", ToArgb ());
#else
				// name is required for serialization under 1.x, but not under 2.0
				if (name == null) {
					// Can happen with stuff deserialized from MS
					if (IsNamedColor)
						name = KnownColors.GetName (knownColor);
					else
						name = String.Format ("{0:x}", ToArgb ());
				}
				return name;
#endif
			}
		}

		public bool IsKnownColor {
			get{
				return (state & ((short) ColorType.Known)) != 0;
			}
		}

		public bool IsSystemColor {
			get{
				return (state & ((short) ColorType.System)) != 0;
			}
		}

		public bool IsNamedColor {
			get{
				return (state & (short)(ColorType.Known|ColorType.Named)) != 0;
			}
		}

		internal long Value {
			get {
				// Optimization for known colors that were deserialized
				// from an MS serialized stream.  
				if (value == 0 && IsKnownColor) {
					value = KnownColors.FromKnownColor ((KnownColor)knownColor).ToArgb () & 0xFFFFFFFF;
				}
				return value;
			}
			set { this.value = value; }
		}

		public static Color_ FromArgb (int red, int green, int blue)
		{
			return FromArgb (255, red, green, blue);
		}
		
		public static Color_ FromArgb (int alpha, int red, int green, int blue)
		{
			CheckARGBValues (alpha, red, green, blue);
			Color_ color = new Color_ ();
			color.state = (short) ColorType.ARGB;
			color.Value = (int)((uint) alpha << 24) + (red << 16) + (green << 8) + blue;
			return color;
		}

		public int ToArgb()
		{
			return (int) Value;
		} 

		public static Color_ FromArgb (int alpha, Color_ baseColor)
		{
			return FromArgb (alpha, baseColor.R, baseColor.G, baseColor.B);
		}

		public static Color_ FromArgb (int argb)
		{
			return FromArgb ((argb >> 24) & 0x0FF, (argb >> 16) & 0x0FF, (argb >> 8) & 0x0FF, argb & 0x0FF);
		}

		public static Color_ FromKnownColor (KnownColor color)
		{
			return KnownColors.FromKnownColor (color);
		}

		public static Color_ FromName (string name)
		{
			try {
				KnownColor kc = (KnownColor) Enum.Parse (typeof (KnownColor), name, true);
				return KnownColors.FromKnownColor (kc);
			}
			catch {
				// This is what it returns! 	 
				Color_ d = FromArgb (0, 0, 0, 0);
				d.name = name;
				d.state |= (short) ColorType.Named;
				return d;
			}
		}

	
		// -----------------------
		// Public Shared Members
		// -----------------------

		/// <summary>
		///	Empty Shared Field
		/// </summary>
		///
		/// <remarks>
		///	An uninitialized Color Structure
		/// </remarks>
		
		public static readonly Color_ Empty;
		
		/// <summary>
		///	Equality Operator
		/// </summary>
		///
		/// <remarks>
		///	Compares two Color objects. The return value is
		///	based on the equivalence of the A,R,G,B properties 
		///	of the two Colors.
		/// </remarks>

		public static bool operator == (Color_ left, Color_ right)
		{
			if (left.Value != right.Value)
				return false;
			if (left.IsNamedColor != right.IsNamedColor)
				return false;
			if (left.IsSystemColor != right.IsSystemColor)
				return false;
			if (left.IsEmpty != right.IsEmpty)
				return false;
			if (left.IsNamedColor) {
				// then both are named (see previous check) and so we need to compare them
				// but otherwise we don't as it kills performance (Name calls String.Format)
				if (left.Name != right.Name)
					return false;
			}
			return true;
		}
		
		/// <summary>
		///	Inequality Operator
		/// </summary>
		///
		/// <remarks>
		///	Compares two Color objects. The return value is
		///	based on the equivalence of the A,R,G,B properties 
		///	of the two colors.
		/// </remarks>

		public static bool operator != (Color_ left, Color_ right)
		{
			return ! (left == right);
		}
		
		public float GetBrightness ()
		{
			byte minval = Math.Min (R, Math.Min (G, B));
			byte maxval = Math.Max (R, Math.Max (G, B));
	
			return (float)(maxval + minval) / 510;
		}

		public float GetSaturation ()
		{
			byte minval = (byte) Math.Min (R, Math.Min (G, B));
			byte maxval = (byte) Math.Max (R, Math.Max (G, B));
			
			if (maxval == minval)
					return 0.0f;

			int sum = maxval + minval;
			if (sum > 255)
				sum = 510 - sum;

			return (float)(maxval - minval) / sum;
		}

		public float GetHue ()
		{
			int r = R;
			int g = G;
			int b = B;
			byte minval = (byte) Math.Min (r, Math.Min (g, b));
			byte maxval = (byte) Math.Max (r, Math.Max (g, b));
			
			if (maxval == minval)
					return 0.0f;
			
			float diff = (float)(maxval - minval);
			float rnorm = (maxval - r) / diff;
			float gnorm = (maxval - g) / diff;
			float bnorm = (maxval - b) / diff;
	
			float hue = 0.0f;
			if (r == maxval) 
				hue = 60.0f * (6.0f + bnorm - gnorm);
			if (g == maxval) 
				hue = 60.0f * (2.0f + rnorm - bnorm);
			if (b  == maxval) 
				hue = 60.0f * (4.0f + gnorm - rnorm);
			if (hue > 360.0f) 
				hue = hue - 360.0f;

			return hue;
		}
		
		// -----------------------
		// Public Instance Members
		// -----------------------

		/// <summary>
		///	ToKnownColor method
		/// </summary>
		///
		/// <remarks>
		///	Returns the KnownColor enum value for this color, 0 if is not known.
		/// </remarks>
		public KnownColor ToKnownColor ()
		{
			return (KnownColor) knownColor;
		}

		/// <summary>
		///	IsEmpty Property
		/// </summary>
		///
		/// <remarks>
		///	Indicates transparent black. R,G,B = 0; A=0?
		/// </remarks>
		
		public bool IsEmpty 
		{
			get {
				return state == (short) ColorType.Empty;
			}
		}

		public byte A {
			get { return (byte) (Value >> 24); }
		}

		public byte R {
			get { return (byte) (Value >> 16); }
		}

		public byte G {
			get { return (byte) (Value >> 8); }
		}

		public byte B {
			get { return (byte) Value; }
		}

		/// <summary>
		///	Equals Method
		/// </summary>
		///
		/// <remarks>
		///	Checks equivalence of this Color and another object.
		/// </remarks>
		
		public override bool Equals (object obj)
		{
			if (!(obj is Color_))
				return false;
			Color_ c = (Color_) obj;
			return this == c;
		}

		/// <summary>
		///	Reference Equals Method
		///	Is commented out because this is handled by the base class.
		///	TODO: Is it correct to let the base class handel reference equals
		/// </summary>
		///
		/// <remarks>
		///	Checks equivalence of this Color and another object.
		/// </remarks>
		//public bool ReferenceEquals (object o)
		//{
		//	if (!(o is Color))return false;
		//	return (this == (Color) o);
		//}



		/// <summary>
		///	GetHashCode Method
		/// </summary>
		///
		/// <remarks>
		///	Calculates a hashing value.
		/// </remarks>
		
		public override int GetHashCode ()
		{
			int hc = (int)(Value ^ (Value >> 32) ^ state ^ (knownColor >> 16));
			if (IsNamedColor)
				hc ^= Name.GetHashCode ();
			return hc;
		}

		/// <summary>
		///	ToString Method
		/// </summary>
		///
		/// <remarks>
		///	Formats the Color as a string in ARGB notation.
		/// </remarks>
		
		public override string ToString ()
		{
			if (IsEmpty)
				return "Color [Empty]";

			// Use the property here, not the field.
			if (IsNamedColor)
				return "Color [" + Name + "]";

			return String.Format ("Color [A={0}, R={1}, G={2}, B={3}]", A, R, G, B);
		}
 
		private static void CheckRGBValues (int red,int green,int blue)
		{
			if( (red > 255) || (red < 0))
				throw CreateColorArgumentException(red, "red");
			if( (green > 255) || (green < 0))
				throw CreateColorArgumentException (green, "green");
			if( (blue > 255) || (blue < 0))
				throw CreateColorArgumentException (blue, "blue");
		}

		private static ArgumentException CreateColorArgumentException (int value, string color)
		{
			return new ArgumentException (string.Format ("'{0}' is not a valid"
				+ " value for '{1}'. '{1}' should be greater or equal to 0 and"
				+ " less than or equal to 255.", value, color));
		}

		private static void CheckARGBValues (int alpha,int red,int green,int blue)
		{
			if( (alpha > 255) || (alpha < 0))
				throw CreateColorArgumentException (alpha, "alpha");
			CheckRGBValues(red,green,blue);
		}


		static public Color_ Transparent {
			get { return KnownColors.FromKnownColor (KnownColor.Transparent); }
		}

		static public Color_ AliceBlue {
			get { return KnownColors.FromKnownColor (KnownColor.AliceBlue); }
		}

		static public Color_ AntiqueWhite {
			get { return KnownColors.FromKnownColor (KnownColor.AntiqueWhite); }
		}

		static public Color_ Aqua {
			get { return KnownColors.FromKnownColor (KnownColor.Aqua); }
		}

		static public Color_ Aquamarine {
			get { return KnownColors.FromKnownColor (KnownColor.Aquamarine); }
		}

		static public Color_ Azure {
			get { return KnownColors.FromKnownColor (KnownColor.Azure); }
		}

		static public Color_ Beige {
			get { return KnownColors.FromKnownColor (KnownColor.Beige); }
		}

		static public Color_ Bisque {
			get { return KnownColors.FromKnownColor (KnownColor.Bisque); }
		}

		static public Color_ Black {
			get { return KnownColors.FromKnownColor (KnownColor.Black); }
		}

		static public Color_ BlanchedAlmond {
			get { return KnownColors.FromKnownColor (KnownColor.BlanchedAlmond); }
		}

		static public Color_ Blue {
			get { return KnownColors.FromKnownColor (KnownColor.Blue); }
		}

		static public Color_ BlueViolet {
			get { return KnownColors.FromKnownColor (KnownColor.BlueViolet); }
		}

		static public Color_ Brown {
			get { return KnownColors.FromKnownColor (KnownColor.Brown); }
		}

		static public Color_ BurlyWood {
			get { return KnownColors.FromKnownColor (KnownColor.BurlyWood); }
		}

		static public Color_ CadetBlue {
			get { return KnownColors.FromKnownColor (KnownColor.CadetBlue); }
		}

		static public Color_ Chartreuse {
			get { return KnownColors.FromKnownColor (KnownColor.Chartreuse); }
		}

		static public Color_ Chocolate {
			get { return KnownColors.FromKnownColor (KnownColor.Chocolate); }
		}

		static public Color_ Coral {
			get { return KnownColors.FromKnownColor (KnownColor.Coral); }
		}

		static public Color_ CornflowerBlue {
			get { return KnownColors.FromKnownColor (KnownColor.CornflowerBlue); }
		}

		static public Color_ Cornsilk {
			get { return KnownColors.FromKnownColor (KnownColor.Cornsilk); }
		}

		static public Color_ Crimson {
			get { return KnownColors.FromKnownColor (KnownColor.Crimson); }
		}

		static public Color_ Cyan {
			get { return KnownColors.FromKnownColor (KnownColor.Cyan); }
		}

		static public Color_ DarkBlue {
			get { return KnownColors.FromKnownColor (KnownColor.DarkBlue); }
		}

		static public Color_ DarkCyan {
			get { return KnownColors.FromKnownColor (KnownColor.DarkCyan); }
		}

		static public Color_ DarkGoldenrod {
			get { return KnownColors.FromKnownColor (KnownColor.DarkGoldenrod); }
		}

		static public Color_ DarkGray {
			get { return KnownColors.FromKnownColor (KnownColor.DarkGray); }
		}

		static public Color_ DarkGreen {
			get { return KnownColors.FromKnownColor (KnownColor.DarkGreen); }
		}

		static public Color_ DarkKhaki {
			get { return KnownColors.FromKnownColor (KnownColor.DarkKhaki); }
		}

		static public Color_ DarkMagenta {
			get { return KnownColors.FromKnownColor (KnownColor.DarkMagenta); }
		}

		static public Color_ DarkOliveGreen {
			get { return KnownColors.FromKnownColor (KnownColor.DarkOliveGreen); }
		}

		static public Color_ DarkOrange {
			get { return KnownColors.FromKnownColor (KnownColor.DarkOrange); }
		}

		static public Color_ DarkOrchid {
			get { return KnownColors.FromKnownColor (KnownColor.DarkOrchid); }
		}

		static public Color_ DarkRed {
			get { return KnownColors.FromKnownColor (KnownColor.DarkRed); }
		}

		static public Color_ DarkSalmon {
			get { return KnownColors.FromKnownColor (KnownColor.DarkSalmon); }
		}

		static public Color_ DarkSeaGreen {
			get { return KnownColors.FromKnownColor (KnownColor.DarkSeaGreen); }
		}

		static public Color_ DarkSlateBlue {
			get { return KnownColors.FromKnownColor (KnownColor.DarkSlateBlue); }
		}

		static public Color_ DarkSlateGray {
			get { return KnownColors.FromKnownColor (KnownColor.DarkSlateGray); }
		}

		static public Color_ DarkTurquoise {
			get { return KnownColors.FromKnownColor (KnownColor.DarkTurquoise); }
		}

		static public Color_ DarkViolet {
			get { return KnownColors.FromKnownColor (KnownColor.DarkViolet); }
		}

		static public Color_ DeepPink {
			get { return KnownColors.FromKnownColor (KnownColor.DeepPink); }
		}

		static public Color_ DeepSkyBlue {
			get { return KnownColors.FromKnownColor (KnownColor.DeepSkyBlue); }
		}

		static public Color_ DimGray {
			get { return KnownColors.FromKnownColor (KnownColor.DimGray); }
		}

		static public Color_ DodgerBlue {
			get { return KnownColors.FromKnownColor (KnownColor.DodgerBlue); }
		}

		static public Color_ Firebrick {
			get { return KnownColors.FromKnownColor (KnownColor.Firebrick); }
		}

		static public Color_ FloralWhite {
			get { return KnownColors.FromKnownColor (KnownColor.FloralWhite); }
		}

		static public Color_ ForestGreen {
			get { return KnownColors.FromKnownColor (KnownColor.ForestGreen); }
		}

		static public Color_ Fuchsia {
			get { return KnownColors.FromKnownColor (KnownColor.Fuchsia); }
		}

		static public Color_ Gainsboro {
			get { return KnownColors.FromKnownColor (KnownColor.Gainsboro); }
		}

		static public Color_ GhostWhite {
			get { return KnownColors.FromKnownColor (KnownColor.GhostWhite); }
		}

		static public Color_ Gold {
			get { return KnownColors.FromKnownColor (KnownColor.Gold); }
		}

		static public Color_ Goldenrod {
			get { return KnownColors.FromKnownColor (KnownColor.Goldenrod); }
		}

		static public Color_ Gray {
			get { return KnownColors.FromKnownColor (KnownColor.Gray); }
		}

		static public Color_ Green {
			get { return KnownColors.FromKnownColor (KnownColor.Green); }
		}

		static public Color_ GreenYellow {
			get { return KnownColors.FromKnownColor (KnownColor.GreenYellow); }
		}

		static public Color_ Honeydew {
			get { return KnownColors.FromKnownColor (KnownColor.Honeydew); }
		}

		static public Color_ HotPink {
			get { return KnownColors.FromKnownColor (KnownColor.HotPink); }
		}

		static public Color_ IndianRed {
			get { return KnownColors.FromKnownColor (KnownColor.IndianRed); }
		}

		static public Color_ Indigo {
			get { return KnownColors.FromKnownColor (KnownColor.Indigo); }
		}

		static public Color_ Ivory {
			get { return KnownColors.FromKnownColor (KnownColor.Ivory); }
		}

		static public Color_ Khaki {
			get { return KnownColors.FromKnownColor (KnownColor.Khaki); }
		}

		static public Color_ Lavender {
			get { return KnownColors.FromKnownColor (KnownColor.Lavender); }
		}

		static public Color_ LavenderBlush {
			get { return KnownColors.FromKnownColor (KnownColor.LavenderBlush); }
		}

		static public Color_ LawnGreen {
			get { return KnownColors.FromKnownColor (KnownColor.LawnGreen); }
		}

		static public Color_ LemonChiffon {
			get { return KnownColors.FromKnownColor (KnownColor.LemonChiffon); }
		}

		static public Color_ LightBlue {
			get { return KnownColors.FromKnownColor (KnownColor.LightBlue); }
		}

		static public Color_ LightCoral {
			get { return KnownColors.FromKnownColor (KnownColor.LightCoral); }
		}

		static public Color_ LightCyan {
			get { return KnownColors.FromKnownColor (KnownColor.LightCyan); }
		}

		static public Color_ LightGoldenrodYellow {
			get { return KnownColors.FromKnownColor (KnownColor.LightGoldenrodYellow); }
		}

		static public Color_ LightGreen {
			get { return KnownColors.FromKnownColor (KnownColor.LightGreen); }
		}

		static public Color_ LightGray {
			get { return KnownColors.FromKnownColor (KnownColor.LightGray); }
		}

		static public Color_ LightPink {
			get { return KnownColors.FromKnownColor (KnownColor.LightPink); }
		}

		static public Color_ LightSalmon {
			get { return KnownColors.FromKnownColor (KnownColor.LightSalmon); }
		}

		static public Color_ LightSeaGreen {
			get { return KnownColors.FromKnownColor (KnownColor.LightSeaGreen); }
		}

		static public Color_ LightSkyBlue {
			get { return KnownColors.FromKnownColor (KnownColor.LightSkyBlue); }
		}

		static public Color_ LightSlateGray {
			get { return KnownColors.FromKnownColor (KnownColor.LightSlateGray); }
		}

		static public Color_ LightSteelBlue {
			get { return KnownColors.FromKnownColor (KnownColor.LightSteelBlue); }
		}

		static public Color_ LightYellow {
			get { return KnownColors.FromKnownColor (KnownColor.LightYellow); }
		}

		static public Color_ Lime {
			get { return KnownColors.FromKnownColor (KnownColor.Lime); }
		}

		static public Color_ LimeGreen {
			get { return KnownColors.FromKnownColor (KnownColor.LimeGreen); }
		}

		static public Color_ Linen {
			get { return KnownColors.FromKnownColor (KnownColor.Linen); }
		}

		static public Color_ Magenta {
			get { return KnownColors.FromKnownColor (KnownColor.Magenta); }
		}

		static public Color_ Maroon {
			get { return KnownColors.FromKnownColor (KnownColor.Maroon); }
		}

		static public Color_ MediumAquamarine {
			get { return KnownColors.FromKnownColor (KnownColor.MediumAquamarine); }
		}

		static public Color_ MediumBlue {
			get { return KnownColors.FromKnownColor (KnownColor.MediumBlue); }
		}

		static public Color_ MediumOrchid {
			get { return KnownColors.FromKnownColor (KnownColor.MediumOrchid); }
		}

		static public Color_ MediumPurple {
			get { return KnownColors.FromKnownColor (KnownColor.MediumPurple); }
		}

		static public Color_ MediumSeaGreen {
			get { return KnownColors.FromKnownColor (KnownColor.MediumSeaGreen); }
		}

		static public Color_ MediumSlateBlue {
			get { return KnownColors.FromKnownColor (KnownColor.MediumSlateBlue); }
		}

		static public Color_ MediumSpringGreen {
			get { return KnownColors.FromKnownColor (KnownColor.MediumSpringGreen); }
		}

		static public Color_ MediumTurquoise {
			get { return KnownColors.FromKnownColor (KnownColor.MediumTurquoise); }
		}

		static public Color_ MediumVioletRed {
			get { return KnownColors.FromKnownColor (KnownColor.MediumVioletRed); }
		}

		static public Color_ MidnightBlue {
			get { return KnownColors.FromKnownColor (KnownColor.MidnightBlue); }
		}

		static public Color_ MintCream {
			get { return KnownColors.FromKnownColor (KnownColor.MintCream); }
		}

		static public Color_ MistyRose {
			get { return KnownColors.FromKnownColor (KnownColor.MistyRose); }
		}

		static public Color_ Moccasin {
			get { return KnownColors.FromKnownColor (KnownColor.Moccasin); }
		}

		static public Color_ NavajoWhite {
			get { return KnownColors.FromKnownColor (KnownColor.NavajoWhite); }
		}

		static public Color_ Navy {
			get { return KnownColors.FromKnownColor (KnownColor.Navy); }
		}

		static public Color_ OldLace {
			get { return KnownColors.FromKnownColor (KnownColor.OldLace); }
		}

		static public Color_ Olive {
			get { return KnownColors.FromKnownColor (KnownColor.Olive); }
		}

		static public Color_ OliveDrab {
			get { return KnownColors.FromKnownColor (KnownColor.OliveDrab); }
		}

		static public Color_ Orange {
			get { return KnownColors.FromKnownColor (KnownColor.Orange); }
		}

		static public Color_ OrangeRed {
			get { return KnownColors.FromKnownColor (KnownColor.OrangeRed); }
		}

		static public Color_ Orchid {
			get { return KnownColors.FromKnownColor (KnownColor.Orchid); }
		}

		static public Color_ PaleGoldenrod {
			get { return KnownColors.FromKnownColor (KnownColor.PaleGoldenrod); }
		}

		static public Color_ PaleGreen {
			get { return KnownColors.FromKnownColor (KnownColor.PaleGreen); }
		}

		static public Color_ PaleTurquoise {
			get { return KnownColors.FromKnownColor (KnownColor.PaleTurquoise); }
		}

		static public Color_ PaleVioletRed {
			get { return KnownColors.FromKnownColor (KnownColor.PaleVioletRed); }
		}

		static public Color_ PapayaWhip {
			get { return KnownColors.FromKnownColor (KnownColor.PapayaWhip); }
		}

		static public Color_ PeachPuff {
			get { return KnownColors.FromKnownColor (KnownColor.PeachPuff); }
		}

		static public Color_ Peru {
			get { return KnownColors.FromKnownColor (KnownColor.Peru); }
		}

		static public Color_ Pink {
			get { return KnownColors.FromKnownColor (KnownColor.Pink); }
		}

		static public Color_ Plum {
			get { return KnownColors.FromKnownColor (KnownColor.Plum); }
		}

		static public Color_ PowderBlue {
			get { return KnownColors.FromKnownColor (KnownColor.PowderBlue); }
		}

		static public Color_ Purple {
			get { return KnownColors.FromKnownColor (KnownColor.Purple); }
		}

		static public Color_ Red {
			get { return KnownColors.FromKnownColor (KnownColor.Red); }
		}

		static public Color_ RosyBrown {
			get { return KnownColors.FromKnownColor (KnownColor.RosyBrown); }
		}

		static public Color_ RoyalBlue {
			get { return KnownColors.FromKnownColor (KnownColor.RoyalBlue); }
		}

		static public Color_ SaddleBrown {
			get { return KnownColors.FromKnownColor (KnownColor.SaddleBrown); }
		}

		static public Color_ Salmon {
			get { return KnownColors.FromKnownColor (KnownColor.Salmon); }
		}

		static public Color_ SandyBrown {
			get { return KnownColors.FromKnownColor (KnownColor.SandyBrown); }
		}

		static public Color_ SeaGreen {
			get { return KnownColors.FromKnownColor (KnownColor.SeaGreen); }
		}

		static public Color_ SeaShell {
			get { return KnownColors.FromKnownColor (KnownColor.SeaShell); }
		}

		static public Color_ Sienna {
			get { return KnownColors.FromKnownColor (KnownColor.Sienna); }
		}

		static public Color_ Silver {
			get { return KnownColors.FromKnownColor (KnownColor.Silver); }
		}

		static public Color_ SkyBlue {
			get { return KnownColors.FromKnownColor (KnownColor.SkyBlue); }
		}

		static public Color_ SlateBlue {
			get { return KnownColors.FromKnownColor (KnownColor.SlateBlue); }
		}

		static public Color_ SlateGray {
			get { return KnownColors.FromKnownColor (KnownColor.SlateGray); }
		}

		static public Color_ Snow {
			get { return KnownColors.FromKnownColor (KnownColor.Snow); }
		}

		static public Color_ SpringGreen {
			get { return KnownColors.FromKnownColor (KnownColor.SpringGreen); }
		}

		static public Color_ SteelBlue {
			get { return KnownColors.FromKnownColor (KnownColor.SteelBlue); }
		}

		static public Color_ Tan {
			get { return KnownColors.FromKnownColor (KnownColor.Tan); }
		}

		static public Color_ Teal {
			get { return KnownColors.FromKnownColor (KnownColor.Teal); }
		}

		static public Color_ Thistle {
			get { return KnownColors.FromKnownColor (KnownColor.Thistle); }
		}

		static public Color_ Tomato {
			get { return KnownColors.FromKnownColor (KnownColor.Tomato); }
		}

		static public Color_ Turquoise {
			get { return KnownColors.FromKnownColor (KnownColor.Turquoise); }
		}

		static public Color_ Violet {
			get { return KnownColors.FromKnownColor (KnownColor.Violet); }
		}

		static public Color_ Wheat {
			get { return KnownColors.FromKnownColor (KnownColor.Wheat); }
		}

		static public Color_ White {
			get { return KnownColors.FromKnownColor (KnownColor.White); }
		}

		static public Color_ WhiteSmoke {
			get { return KnownColors.FromKnownColor (KnownColor.WhiteSmoke); }
		}

		static public Color_ Yellow {
			get { return KnownColors.FromKnownColor (KnownColor.Yellow); }
		}

		static public Color_ YellowGreen {
			get { return KnownColors.FromKnownColor (KnownColor.YellowGreen); }
		}
	}
}
