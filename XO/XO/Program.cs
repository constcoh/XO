using System;
using System.Drawing;
using System.Windows.Forms;
using XO;

class Constants
{
    public const int buttonSize = 40; // Размер кнопки
    public const int stepSize = 2 * buttonSize; // Отступ для следующей кнопки
    public const int margin = 5; // Отступ для следующей кнопки
    public static readonly Size button2dSize = new Size(Constants.buttonSize - 2 * Constants.margin, Constants.buttonSize - 2 * Constants.margin);
}

namespace GraphicsObject_c
{
    /// <summary>
    /// Summary description for GraphicsObject.
    /// </summary>
    public class GraphicsObject : Form
    {
        private Button BShowField;
        private Button BClearField;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        Panel[,] panels = new Panel[3, 3];
        
        public GraphicsObject()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            this.CreatePanels();
        }

        public void drawXO(int iCoef, int jCoef)
        {
            using (Graphics G2 = this.CreateGraphics())
            {
                if (player == Player.X)
                {
                    G2.DrawLine(new Pen(Color.Black, 2), jCoef * Constants.buttonSize, iCoef * Constants.buttonSize, (jCoef + 1) * Constants.buttonSize, (iCoef + 1) * Constants.buttonSize);
                    G2.DrawLine(new Pen(Color.Black, 2), (jCoef + 1) * Constants.buttonSize, iCoef * Constants.buttonSize, jCoef * Constants.buttonSize, (iCoef + 1) * Constants.buttonSize);
                }
                else
                {
                    // Create pen.
                    Pen blackPen = new Pen(Color.Black, 3);

                    // Create rectangle for ellipse.
                    Rectangle rect = new Rectangle(jCoef * Constants.buttonSize, iCoef * Constants.buttonSize, Constants.buttonSize, Constants.buttonSize);

                    // Draw ellipse to screen.
                    G2.DrawEllipse(blackPen, rect);
                }
            }
        }

        private class PanelClicker
        {
            private readonly int _x;
            private readonly int _y;
            private readonly GraphicsObject _graphicsObject;

            public PanelClicker(int x, int y, GraphicsObject graphicsObject)
            {
                _x = x;
                _y = y;
                _graphicsObject = graphicsObject;
            }

            public void panel_Click()
            {
                _graphicsObject.panels[_x, _y].Dispose();

                // Координаты клетки
                int iCoef = _y, jCoef = _x;

                _graphicsObject.drawXO(iCoef, jCoef);

                _graphicsObject.xofield[_x, _y] = (int)_graphicsObject.player; // Ставим знак (Ставим цифру в поле). 1 - Player.X, 2 - Player.O
                _graphicsObject.player = _graphicsObject.player.Reverse(); // Передаём ход другому игроку

                _graphicsObject.checkWinner();
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BShowField = new Button();
            this.BClearField = new Button();

            this.SuspendLayout();

            int x = 0;
            int y = 0;

            // 
            // BShowField (Показать поле в численном виде)
            // 
            this.BShowField.Location = new Point(x + 4 * Constants.stepSize, y);
            this.BShowField.Name = "BShowField";
            this.BShowField.Size = new Size(100, 100);
            this.BShowField.TabIndex = 3;
            this.BShowField.Text = "Show field";
            this.BShowField.Click += new EventHandler(this.BShowField_Click);
            // 
            // BClearField (Обнулить поле)
            // 
            this.BClearField.Location = new Point(x + 4 * Constants.stepSize, 100 + y);
            this.BClearField.Name = "BClearField";
            this.BClearField.Size = new Size(100, 100);
            this.BClearField.TabIndex = 3;
            this.BClearField.Text = "Clear";
            this.BClearField.Click += new EventHandler(this.BClearField_Click);



            // 
            // GraphicsObject
            // 
            this.AutoScaleBaseSize = new Size(5, 13);
            this.ClientSize = new Size(424, 373);
            this.Controls.AddRange(new Control[] {
                                                                this.BShowField,
                                                                this.BClearField});
            this.Name = "GraphicsObject";
            this.Text = "GraphicsObject";
            this.Load += new EventHandler(this.GraphicsObject_Load);
            this.ResumeLayout(false);

        }
        #endregion

        int[,] xofield = new int[,] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } }; // 0 - пустая клетка, 1 - Player.X, 2 - Player.O
        Player player = Player.X;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new GraphicsObject());
        }

        private void GraphicsObject_Load(object sender, System.EventArgs e)
        {

        }

        //----------------------------------------------------------------------------

        ////private void Buttons_Click(object sender, System.EventArgs e)
        ////{
        ////    Button b = (Button)sender;

        ////    // Клетка ещё пустая
        ////    if (b.Text != Player.X && b.Text != Player.O)
        ////    {
        ////        b.Text = player; // Какой знак ставим

        ////        // Из имени кнопки получаем координаты в поле
        ////        xofield[b.Name[1] - '0', b.Name[2] - '0'] = player == Player.X ? 1 : 2; // Ставим знак (Ставим цифру в поле). 1 - Player.X, 2 - Player.O
        ////        player = player == Player.X ? Player.O : Player.X; // Передаём ход другому игроку
        ////    }
        ////}

        public void checkWinner()
        {
            int x1 = 0,
                y1 = 0,

                x2 = 0,
                y2 = 0;

            // Рисовать линию или нет
            bool doDraw = false;

            // Проходим по строкам
            for (int i = 0; i < 3; i++)
            {
                if (xofield[i, 0] == xofield[i, 1] && xofield[i, 1] == xofield[i, 2] && xofield[i, 0] != 0)
                {
                    x1 = 0;
                    y1 = i * Constants.buttonSize + Constants.buttonSize / 2;

                    x2 = 3 * Constants.buttonSize;
                    y2 = y1;

                    doDraw = true;
                    break;
                }
            }

            // Проходим по столбцам
            for (int j = 0; j < 3; j++)
            {
                if (xofield[0, j] == xofield[1, j] && xofield[1, j] == xofield[2, j] && xofield[0, j] != 0)
                {
                    x1 = j * Constants.buttonSize + Constants.buttonSize / 2;
                    y1 = 0;

                    x2 = x1;
                    y2 = 3 * Constants.buttonSize;

                    doDraw = true;
                    break;
                }
            }

            // Главная диагональ
            if (xofield[0, 0] == xofield[1, 1] && xofield[0, 0] == xofield[2, 2] && xofield[0, 0] != 0)
            {
                x2 = 3 * Constants.buttonSize;
                y2 = 3 * Constants.buttonSize;

                doDraw = true;
            }

            // Побочная диагональ
            if (xofield[0, 2] == xofield[1, 1] && xofield[1, 1] == xofield[2, 0] && xofield[0, 2] != 0)
            {
                x1 = 3 * Constants.buttonSize;
                y2 = 3 * Constants.buttonSize;

                doDraw = true;
            }

            if (doDraw)
            {
                // Рисуем линию
                Graphics G;

                G = this.CreateGraphics();
                G.DrawLine(new Pen(Color.DarkMagenta, 10), y1, x1, y2, x2);

                G.Dispose();
            }
        }

        // Проверка, что поле заполняется правильно
        private void BShowField_Click(object sender, System.EventArgs e)
        {
            Button b = (Button)sender;
            b.Text = "Show field\n" +
                xofield[0, 0].ToString() + "  " + xofield[0, 1].ToString() + "  " + xofield[0, 2].ToString() + "\n" +
                xofield[1, 0].ToString() + "  " + xofield[1, 1].ToString() + "  " + xofield[1, 2].ToString() + "\n" +
                xofield[2, 0].ToString() + "  " + xofield[2, 1].ToString() + "  " + xofield[2, 2].ToString()
                ;


            Graphics G2;
            G2 = this.CreateGraphics();
            G2.DrawLine(new Pen(Color.DarkMagenta, 2), 0 * Constants.buttonSize, 0, 0 * Constants.buttonSize, 3 * Constants.buttonSize);
            G2.DrawLine(new Pen(Color.DarkMagenta, 2), 1 * Constants.buttonSize, 0, 1 * Constants.buttonSize, 3 * Constants.buttonSize);
            G2.DrawLine(new Pen(Color.DarkMagenta, 2), 2 * Constants.buttonSize, 0, 2 * Constants.buttonSize, 3 * Constants.buttonSize);
            G2.DrawLine(new Pen(Color.DarkMagenta, 2), 3 * Constants.buttonSize, 0, 3 * Constants.buttonSize, 3 * Constants.buttonSize);

            G2.DrawLine(new Pen(Color.DarkMagenta, 2), 0, 0 * Constants.buttonSize, 3 * Constants.buttonSize, 0 * Constants.buttonSize);
            G2.DrawLine(new Pen(Color.DarkMagenta, 2), 0, 1 * Constants.buttonSize, 3 * Constants.buttonSize, 1 * Constants.buttonSize);
            G2.DrawLine(new Pen(Color.DarkMagenta, 2), 0, 2 * Constants.buttonSize, 3 * Constants.buttonSize, 2 * Constants.buttonSize);
            G2.DrawLine(new Pen(Color.DarkMagenta, 2), 0, 3 * Constants.buttonSize, 3 * Constants.buttonSize, 3 * Constants.buttonSize);

            G2.Dispose();
        }

        private void BClearField_Click(object sender, System.EventArgs e)
        {
            // Установка дефолтного игрока 
            player = Player.X;

            // Обнуление массива
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    xofield[i, j] = 0;

            // Заливка дефолтным цветом, чтобы убрать нарисованную линию
            Graphics G;
            G = this.CreateGraphics();
            G.Clear(Control.DefaultBackColor);


            this.DisposePanels();
            this.CreatePanels();
        }

        private void CreatePanels()
        {
            for (int x = 0; x < 3; ++x)
            {
                for (int y = 0; y < 3; ++y)
                {
                    panels[x, y] = new Panel();
                    panels[x, y].Location =
                        new Point(
                            x * Constants.buttonSize + Constants.margin,
                            y * Constants.buttonSize + Constants.margin);

                    panels[x, y].Size = Constants.button2dSize;

                    var clicker = new PanelClicker(x, y, this);

                    panels[x, y].Click += new EventHandler((object o, EventArgs e) => {
                        clicker.panel_Click();
                    });

                    this.Controls.Add(this.panels[x, y]);
                }
            }
        }

        private void DisposePanels()
        {
            for (int x = 0; x < 3; ++x)
            {
                for (int y = 0; y < 3; ++y)
                {
                    panels[x, y].Dispose();
                }
            }
        }
    }
}