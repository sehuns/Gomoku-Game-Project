using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class SinglePlayForm : Form
    {
        private const int rectSize = 33; // 오목판 셀 크기
        private const int edgeCount = 15; // 오목판 선 개수

        private enum Horse { none = 0, BLACK, WHITE }; // 말 자료형 만듦, 비어있으면 0
        private Horse[,] board = new Horse[edgeCount, edgeCount]; //특정 위치에 돌 놓여진 사실 처리 배열
        private Horse nowPlayer = Horse.BLACK; // 현재 차례 플레이어를 저장하는 변수? 처음엔 흑돌 선

        private bool playing = false; //게임 진행중임을 판단할 수 있는 함수

        public SinglePlayForm()
        {
            InitializeComponent();
        }


        private bool judge() // 승리 판정 함수
        {
            for (int i = 0; i < edgeCount - 4; i++) // 가로 판정
                for (int j = 0; j < edgeCount; j++)
                    if (board[i, j] == nowPlayer && board[i+1, j] == nowPlayer && board[i+2, j] == nowPlayer &&
                        board[i+3, j] == nowPlayer && board[i+4, j]== nowPlayer)
                        return true;

            for (int i = 0; i < edgeCount; i++) // 세로 판정
                for (int j = 4; j < edgeCount; j++)
                    if (board[i, j] == nowPlayer && board[i, j - 1] == nowPlayer && board[i, j - 2] == nowPlayer &&
                        board[i, j - 3] == nowPlayer && board[i, j - 4] == nowPlayer)
                        return true;

            for (int i = 0; i < edgeCount - 4; i++) // Y = X 직선
                for (int j = 0; j < edgeCount - 4; j++)
                    if (board[i, j] == nowPlayer && board[i + 1, j + 1] == nowPlayer && board[i + 2, j + 2] == nowPlayer &&
                        board[i + 3, j + 3] == nowPlayer && board[i + 4 , j + 4] == nowPlayer)
                        return true;

            for (int i = 4; i < edgeCount; i++) // Y = -X 직선
                for (int j = 0; j < edgeCount - 4; j++)
                    if (board[i, j] == nowPlayer && board[i - 1, j + 1] == nowPlayer && board[i - 2, j + 2] == nowPlayer &&
                        board[i - 3, j + 3] == nowPlayer && board[i - 4, j + 4] == nowPlayer)
                        return true;
            return false;
        }

        private void refresh()
        {
            this.boardPicture.Refresh();
            for (int i = 0; i < edgeCount; i++)
                for (int j = 0; j < edgeCount; j++)
                    board[i, j] = Horse.none;

        }



        private void boardPicture_MouseDown(object sender, MouseEventArgs e)
        { // 오목판을 클릭했을 때

            if (!playing)
            {
                MessageBox.Show("게임을 실행해 주세요.");
                return;
            }

            Graphics g = this.boardPicture.CreateGraphics(); //클릭 시 그림그려주기위해
            int x = e.X / rectSize; // 클릭한 위치가 어디냐
            int y = e.Y / rectSize;
            if (x < 0 || y < 0 || x >= edgeCount || y >= edgeCount)
            {
                MessageBox.Show("테두리를 벗어날 수 없습니다.");
                return;
            }
           

            //MessageBox.Show(x + ", " + y);

            if (board[x, y] != Horse.none) return;
            board[x, y] = nowPlayer;
            if (nowPlayer == Horse.BLACK)
            {
                SolidBrush brush = new SolidBrush(Color.Black);
                g.FillEllipse(brush, x * rectSize, y * rectSize, rectSize, rectSize);
            }
            else
            {
                SolidBrush brush = new SolidBrush(Color.White);
                g.FillEllipse(brush, x * rectSize, y * rectSize, rectSize, rectSize);
            }

            if (judge())
            {
                status.Text = nowPlayer.ToString() + "플레이어가 승리했습니다";
                playing = false;
                playButton.Text = "게임시작";
            }
            else
            {
                nowPlayer = ((nowPlayer == Horse.BLACK) ? Horse.WHITE : Horse.BLACK);
                status.Text = nowPlayer.ToString() + " 플레이어의 차례입니다.";
            }

        }

        private void boardPicture_Paint(object sender, PaintEventArgs e)
        { //오목판 처음 구성, 리프레시할 때마다 다시 그린다? 초기화면 구성 함수
            Graphics gp = e.Graphics;
            Color lineColor = Color.Black; //오목판 선 색
            Pen p = new Pen(lineColor, 2); // 굵기
            gp.DrawLine(p, rectSize / 2, rectSize / 2, rectSize / 2, rectSize * edgeCount - rectSize / 2); // 좌측
            gp.DrawLine(p, rectSize / 2, rectSize / 2, rectSize * edgeCount - rectSize / 2, rectSize / 2); // 상측
            gp.DrawLine(p, rectSize / 2, rectSize * edgeCount - rectSize / 2, rectSize * edgeCount - rectSize / 2, rectSize * edgeCount - rectSize / 2); // 하측
            gp.DrawLine(p, rectSize * edgeCount - rectSize / 2, rectSize / 2, rectSize * edgeCount - rectSize / 2, rectSize * edgeCount - rectSize / 2); // 우측
            p = new Pen(lineColor, 1);
            // 대각선 방향으로 이동하면서 십자가 모양의 선 그리기
            for (int i = rectSize + rectSize / 2; i < rectSize * edgeCount - rectSize / 2; i += rectSize)
            {
                gp.DrawLine(p, rectSize / 2, i, rectSize * edgeCount - rectSize / 2, i);
                gp.DrawLine(p, i, rectSize / 2, i, rectSize * edgeCount - rectSize / 2);
            }

        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (!playing)
            {
                refresh();
                playing = true;
                playButton.Text = "재시작";
                status.Text = nowPlayer.ToString() + " 플레이어의 차례입니다.";
            }
            else
            {
                refresh();
                status.Text = "게임이 재시작되었습니다.";
            }

        }
    }
}
