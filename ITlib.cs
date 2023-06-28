using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerPower
{
    public class ImageText
    {
        //our one constant
        private static readonly string defaultfont = "Courier New"; //default font is Courier New


        //variables
        public Image image; //The one REQUIRED variable
        private PointF position = new(0, 0); //position defaults to 0, 0
        private RectangleF margins = new(0, 0, 0, 0); //margins defaults to none
        private string fontname = defaultfont; //fontname defaults to defaultfont
        private FontFamily fontfamily = new(defaultfont); //fontfamily defaults to using default font
        private float fontsize = 16; //fontsize defaults to 16
        private FontStyle fontstyle = FontStyle.Regular; //fontstyle defaults to regular
        private Font font = new(defaultfont, 16); //font defaults to default font at fontsize 16
        private Color forecolor = Color.Black; //foreground color defaults to black
        private Color backcolor = Color.White; //background color defaults to white
        private Color outlinecolor = Color.Gray; //outline color defaults to gray
        private float outlinesize = 1; //outline size defaults to 1



        //constructors
        public ImageText(Image image) => this.image = image; //image set
        public ImageText(PictureBox pictureBox) => image = pictureBox.Image; //get image from picturebox control



        //set / get parameters
        public Image GetImage() => this.image;
        public void SetImage(Image image) => this.image = image;
        public void SetImage(PictureBox pictureBox) => image = pictureBox.Image;
        public PointF GetPosition() => position;
        public void SetPosition(PointF position) => this.position = position;
        public void SetPosition(float horizontal, float vertical) => this.position = new PointF(horizontal, vertical);
        public void SetPosition() => position = new(0, 0);
        public RectangleF GetMargins() => margins;
        public void SetMargins(RectangleF margins) => this.margins = margins;
        public void SetMargins(float left, float top, float right, float bottom) => this.margins = new RectangleF(left, top, right, bottom);
        public void SetMargins() => margins = new(0, 0, 0, 0);
        public string GetFontName() => fontname;
        public void SetFontName(string fontname)
        {
            this.fontname = fontname; //set fontname
            fontfamily.Dispose(); //get rid of fontfamily
            fontfamily = new FontFamily(fontname); //set fontfamily
            font.Dispose(); //get rid of font
            font = new Font(fontname, fontsize, fontstyle); //set up font
        }
        public FontFamily GetFontFamily() => fontfamily;
        public void SetFontFamily(FontFamily fontfamily)
        {
            this.fontfamily = fontfamily; //set fontfamily
            font.Dispose(); //get rid of font
            font = new Font(fontfamily, fontsize, fontstyle); //set up font
        }
        public void SetFontFamily(string fontname) => SetFontName(fontname);
        public float GetFontSize() => fontsize;
        public void SetFontSize(float fontsize)
        {
            this.fontsize = fontsize; //set fontsize
            font.Dispose(); //get rid of font
            font = new Font(fontname, fontsize, fontstyle); //set up font
        }
        public Font GetFont() => font;
        public FontStyle GetFontStyle() => fontstyle;
        public void SetFontStyle(FontStyle fontstyle)
        {
            this.fontstyle = fontstyle; //set fontstyle
            font.Dispose(); //get rid of font
            font = new Font(fontname, fontsize, fontstyle); //set up font
        }
        public void SetFont(Font font)
        {
            this.font = font; //set font
            fontfamily.Dispose(); //get rid of fontfamily
            fontfamily = font.FontFamily; //set fontfamily
            fontsize = font.Size; //set font size
            fontstyle = font.Style; //set font style
        }
        public void SetFont(string fontname, float fontsize = 16, FontStyle fontstyle = FontStyle.Regular)
        {
            this.fontname = fontname; //set font name
            fontfamily.Dispose(); //get rid of fontfamily
            fontfamily = new FontFamily(fontname); //set fontfamily
            this.fontsize = fontsize; //set font size
            this.fontstyle = fontstyle; //set font style
            font.Dispose(); //get rid of font
            font = new(fontname, fontsize, fontstyle); //set font
        }
        public Color GetForeColor() => forecolor;
        public void SetForeColor(Color forecolor) => this.forecolor = forecolor;
        public void SetForeColor(int red, int green, int blue, int alpha = 255) => forecolor = Color.FromArgb(red, green, blue, alpha);
        public Color GetBackColor() => backcolor;
        public void SetBackColor(Color backcolor) => this.backcolor = backcolor;
        public void SetBackColor(int red, int green, int blue, int alpha = 255) => backcolor = Color.FromArgb(red, green, blue, alpha);
        public Color GetOutlineColor() => outlinecolor;
        public void SetOutlineColor(Color outlinecolor) => this.outlinecolor = outlinecolor;
        public void SetOutlineColor(int red, int green, int blue, int alpha = 255) => outlinecolor = Color.FromArgb(red, green, blue, alpha);
        public float GetOutlineSize() => outlinesize;
        public void SetOutlineSize(float outlinesize) => this.outlinesize = outlinesize;



        //basic work
        public void Draw(string text, bool clear = false, bool breakonspace = true, bool outline = false)
        {
            PointF newpos = new(margins.Left + position.X, margins.Top + position.Y); //newpos is position with margins
            PointF edgepos = new(image.Width - margins.Width, image.Height - margins.Height); //edgepos is image size with margins
            string beforespace, afterspace; //variables for portions of text before and after breaking space
            SizeF size = GetActualSizeText(text, outline); //get size of text
            if ((!breakonspace) || (newpos.X + size.Width < edgepos.X)) //if not breakonspace or size doesn't put us past the edge
            {
                DrawIt(text, clear, outline); //simply draw it
                return; //exit function
            }
            beforespace = text; //set beforespace to text
            while (newpos.X + size.Width >= edgepos.X) //while too big for width available
            {
                beforespace = beforespace[..beforespace.LastIndexOf(" ")]; //set beforespace to be up to the last space
                size = GetActualSizeText(beforespace, outline); //get the size of beforespace
            }
            afterspace = text.Remove(0, beforespace.Length); //set afterspace to text without beforespace
            if (clear) ClearBackground(size, newpos); //if clear, clear background
            DrawActualText(beforespace, newpos, outline); //draw beforespace text
            CarriageReturn(); //do a carriage return
            Draw(afterspace, clear, breakonspace, outline); //recursive call to handle afterspace
        }
        public void DrawIt(string text, bool clear = false, bool outline = false)
        {
            if (outline) DrawOutlineText(text, clear); //if outline, draw text with outline
            else DrawText(text, clear); //otherwise just draw text
        }
        public void DrawText(string text, bool clear = false)
        {
            PointF newpos = new(margins.Left + position.X, margins.Top + position.Y); //get position with margins
            SizeF size = GetTextSize(text); //get text size
            if (clear) ClearBackground(size, newpos); //if clear, clear background
            DrawString(text, newpos); //draw text at new position
            position.X += size.Width; //adjust position
        }

        public void DrawOutlineText(string text, bool clear = false)
        {
            PointF newpos = new(margins.Left + position.X, margins.Top + position.Y); //get position with margins
            SizeF size = GetOutlineTextSize(text); //get text size
            if (clear) ClearBackground(size, newpos); //if clear, clear background
            DrawOutlineString(text, newpos); //draw text at new position
            position.X += size.Width; //adjust position
        }

        public void CarriageReturn()
        {
            SizeF size = GetTextSize(" "); //get size of a space to use for line height
            position.X = 0; //set horizontal position to beginning
            position.Y += size.Height; //add height to vertical position
        }

        public void DrawCenterText(string text, bool horizontal = true, bool vertical = false, bool clear = false)
        {
            PointF newpos = new(margins.Left + position.X, margins.Top + position.Y); //get position with margins
            SizeF size = GetTextSize(text); //get text size
            if (horizontal) //if horizontal centering
                newpos.X = (image.Width - size.Width) / 2; //set horizontal position accordingly
            if (vertical) //if vertical centering
                newpos.Y = (image.Height - size.Height) / 2; //set vertical position accordingly
            if (clear) ClearBackground(size, newpos); //if clear, clear background
            DrawString(text, newpos); //draw text at new position
            position.X = newpos.X + size.Width - margins.Left; //adjust horizontal position
            position.Y = newpos.Y - margins.Top; //adjust vertical position
        }

        public void DrawCenterOutlineText(string text, bool horizontal = true, bool vertical = false, bool clear = false)
        {
            PointF newpos = new(margins.Left + position.X, margins.Top + position.Y); //get position with margins
            SizeF size = GetOutlineTextSize(text); //get text size
            if (horizontal) //if horizontal centering
                newpos.X = (image.Width - size.Width) / 2; //set horizontal position accordingly
            if (vertical) //if vertical centering
                newpos.Y = (image.Height - size.Height) / 2; //set vertical position accordingly
            if (clear) ClearBackground(size, newpos); //if clear, clear backgeound
            DrawOutlineString(text, newpos); //draw text at new position
            position.X = newpos.X + size.Width - margins.Left; //adjust horizontal position
            position.Y = newpos.Y - margins.Top; //adjust vertical position
        }

        public void DrawRightText(string text, bool clear = false)
        {
            PointF lastpos = new(image.Width - margins.Width, position.Y); //get position of flush to right margin
            SizeF size = GetTextSize(text); //get size of text
            lastpos.X -= size.Width; //subtract width from position
            if (clear) ClearBackground(size, lastpos); //if clear, clear background
            DrawString(text, lastpos); //draw the text at the calculated position
            position.X = 0; //set horizontal position to left
            position.Y += size.Height; //add height to vertical position
        }

        public void DrawRightOutlineText(string text, bool clear = false)
        {
            PointF lastpos = new(image.Width - margins.Width, position.Y); //get position of flush to right margin
            SizeF size = GetOutlineTextSize(text); //get size of text
            lastpos.X -= size.Width; //subtract width from position
            if (clear) ClearBackground(size, lastpos); //if clear, clear background
            DrawOutlineString(text, lastpos); //draw the text at the calculated position
            position.X = 0; //set horizontal position to left
            position.Y += size.Height; //add height to vertical position
        }

        public SizeF GetSize(string text, bool outline = false) => GetActualSizeText(text, outline);


        //private worker functions
        private SizeF GetActualSizeText(string text, bool outline = false)
        {
            if (outline) return GetOutlineTextSize(text); //if outline, return outline text size
            else return GetTextSize(text); //otherwise, return text size
        }
        private SizeF GetTextSize(string text)
        {
            Graphics g = Graphics.FromImage(image); //make graphics object
            SizeF size = g.MeasureString(text, font); //measure the size of the text
            g.Dispose(); //dispose of the graphics object
            return size; //return the size
        }
        private void DrawActualText(string text, PointF pos, bool outline = false)
        {
            if (outline) DrawOutlineString(text, pos); //if outline, draw outline text
            else DrawString(text, pos); //otherwise, just draw the text
        }
        private void DrawString(string text, PointF pos)
        {
            Graphics g = Graphics.FromImage(image); //get graphics object
            SolidBrush b = new(forecolor); //set a brush
            g.DrawString(text, font, b, pos); //draw the text
            b.Dispose(); //get rid of brush
            g.Dispose(); //get rid of graphics object
        }
        private SizeF GetOutlineTextSize(string text)
        {
            Graphics g = Graphics.FromImage(image); //get graphics object
            Pen p = new(outlinecolor, outlinesize); //set up pen
            GraphicsPath gp = new(); //make a graphicspath object
            gp.AddString(text, fontfamily, (int)fontstyle, g.DpiY * fontsize / 72, position, new StringFormat()); //add text to graphicspath
            RectangleF rect = gp.GetBounds(null, p); //get size of graphics path object
            gp.Dispose(); //get rid of graphicspath object
            p.Dispose(); //get rid of pen object
            g.Dispose(); //get rid of graphics object
            SizeF size = new(rect.Width, rect.Height); //set size
            return size; //return size
        }
        private void DrawOutlineString(string text, PointF pos)
        {
            Graphics g = Graphics.FromImage(image); //get graphics object
            SolidBrush b = new(forecolor); //make brush
            Pen p = new(outlinecolor, outlinesize); //make pen
            GraphicsPath gp = new(); //make graphics path
            gp.AddString(text, fontfamily, (int)fontstyle, g.DpiY * fontsize / 72, pos, new StringFormat()); //add text to graphicspath object
            g.DrawPath(p, gp); //draw the outline
            g.FillPath(b, gp); //fill the interior
            gp.Dispose(); //get rid of graphicspath object
            p.Dispose(); //get rid of pen
            b.Dispose(); //get rid of brush
            g.Dispose(); //get rid of graphics object
        }
        private void ClearBackground(SizeF size, PointF point)
        {
            Graphics g = Graphics.FromImage(image); //get graphics object
            Pen pen = new(backcolor); //make pen
            SolidBrush brush = new(backcolor); //make brush
            Rectangle rect = new((int)point.X, (int)point.Y, (int)size.Width, (int)size.Height); //set up rectangle
            g.DrawRectangle(pen, rect); //draw rectangle
            g.FillRectangle(brush, rect); //fill rectangle
            pen.Dispose(); //get rid of pen
            brush.Dispose(); //get rid of brush
            g.Dispose(); //get rid of graphics object
        }
    }
}
