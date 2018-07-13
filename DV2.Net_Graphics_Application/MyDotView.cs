#define USE_DIALOG
/**
 * DotView2制御用のクラス
 * DV2の多重接続禁止などを考慮してシングルトンで作成
 *
 * @author Shingo Morii
 * @author Chou Hou
 * @Version 2.0
 */

using System;
using System.Text;

#region Personal Addition
using System.Threading;
using System.Windows.Forms;
using DV = KGS.Tactile.Display;
#endregion

namespace DV2.Net_Graphics_Application
{
    class MyDotView
    {
		// DotViewハード制御関連の変数(我々はいじらない)
		private MainForm mainform;
		private const int GnHardIdx = 2;
		private const int gnPortIdx = 5;
		private System.Windows.Forms.IWin32Window _WindowHandle;
		public DV.Control DvCtl;
		private DV.GVIEW_PANEL_INFO dPanelFixed;
		private DV.GVIEW_PANEL_INFO dPanelBlinking;
		private string EquipmentName;
		private int TextSize;
		private int GrpX;
		private int GrpY;

		// DV2の接続状態
		// true:接続、false:切断
		private bool isConnect = false;

		// このクラスのインスタンス
		private static MyDotView mdv = null;

		// コンストラクタ(シングルトンなのでprivate)
		private MyDotView(MainForm mF)
		{
            mainform = mF;
            //ここは一番大事!
            mainform.Text = "";

            _WindowHandle = mF;
			DvCtl = new DV.Control();
			dPanelFixed = new DV.GVIEW_PANEL_INFO();
			dPanelBlinking = new DV.GVIEW_PANEL_INFO();
			EquipmentName = null;
			TextSize = 0;
			GrpX = 0; GrpY = 0;
		}

		// このクラスのインスタンスを生成して返却する
		// シングルトンなので必ず1度しかインスタンスは作られない
	    public static MyDotView getInstance(MainForm mF)
		{
			// インスタンスが生成されていなければ
			if (mdv == null)
			{
				// インスタンスの新規作成
				mdv = new MyDotView(mF);
			}
			return mdv;
		}

		// DV2の接続と描画領域の初期化
		public void Connect()
		{
			bool bRet = false;
			#if USE_DIALOG
				bRet = DvCtl.SelectStartDialog(_WindowHandle);
            #else
				bRet = DvCtl.DirectStartConnect(_WindowHandle, GnHardIdx, gnPortIdx);
            #endif

            if (!bRet)
            {
                MessageBox.Show("@Connect関数 DotViewの接続に失敗した");
                isConnect = false;
                return;
            }
            bRet = false;
            for (int nCount = 0; nCount < 20; nCount++)
            {
                bRet = DvCtl.QueryEquipment(-1, out EquipmentName, out TextSize, out GrpX, out GrpY);
                if (bRet)
                {
                    break;
                }
                Thread.Sleep(100);
            }
            if (!bRet)
            {
                MessageBox.Show("@Connect関数　DotViewの初期化は失敗した");
                isConnect = false;
                return;
            }

            DvCtl.StartDisplayMode(DV.DispMode.FOREGROUND);
			DvCtl.HideControlBoxWindow(true);

			dPanelFixed = DvCtl.NewPanel(GrpX, GrpY);
			dPanelBlinking = DvCtl.NewPanel(GrpX, GrpY);
			Array.Clear(dPanelFixed.Buffer, 0, dPanelFixed.SizeOfDataBytes);
			Array.Clear(dPanelBlinking.Buffer, 0, dPanelBlinking.SizeOfDataBytes);
			isConnect = true;
			return;
		}

		// DV2の切断とバッファのクリア
		public void Disconnect()
		{
			DvCtl.EndConnect();
			DvCtl.EndDisplayMode();
			DvCtl.FreePanel(dPanelFixed);
			DvCtl.FreePanel(dPanelBlinking);
			isConnect = false;
		}

		// 描画領域の計算
		private void spb(int x, int y, DV.GVIEW_PANEL_INFO lpPanel)
		{
			int ny = y / 8;
			int by = y % 8;
            lpPanel.Buffer[ny * lpPanel.SizeOfXBytes + x] |= (byte)(1 << by);
		}

		// 点を表示する
		// DotView解像度と同じ２次元配列を用意して
		// その配列の値が１ならば固定点をうち
		// ２ならば第２引数でしていする間隔で点滅させる
		public void SetDots(int[,] DotData, int BlinkInterval)
		{
			// DotViewが正しく接続されていれば
			if (isConnect)
			{
				for (int i=0; i<48; i++)
                { 
					for (int j=0; j<32; j++)
					{
                        if (DotData[i, j] == 1)
                            spb(i, j, dPanelFixed);
                        else if (DotData[i, j] == 2)
                            spb(i, j, dPanelBlinking);
					}
                }
                // DotViewへバッファの送信
                DvCtl.DisplayGraphicData(dPanelFixed.Buffer, dPanelBlinking.Buffer, BlinkInterval);
			}
		}
    }
}
