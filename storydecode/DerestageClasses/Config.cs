using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Story.Data
{
	[ClassInterface(ClassInterfaceType.None), ComDefaultInterface(typeof(ICommandConfig)), ComVisible(true)]
	public class Config : ICommandConfig
	{
		private List<CommandConfig> _commandConfigList;

		public static Config Create()
		{
			Config config = new Config();
			config.Init();
			return config;
		}

		public void Init()
		{
			this._commandConfigList = new List<CommandConfig>();
			this.RegisterCommandConfig("title", "タイトル設定", "title <タイトル名>", "Title", CommandCategory.System, 1, 1);
			this.RegisterCommandConfig("outline", "あらすじ設定", "outline <あらすじ>", "Outline", CommandCategory.System, 1, 1);
			this.RegisterCommandConfig("chara", "キャラの追加", "chara <キャラID> <L:左, LC:左寄り, C:真ん中, RC:右寄り, R:右> <タイプID> <表情ID>", "Chara", CommandCategory.System, 4, 4);
			this.RegisterCommandConfig("visible", "キャラ表示切り替え", "visible <キャラID> <true か false>", "Visible", CommandCategory.System, 2, 2);
			this.RegisterCommandConfig("type", "キャラタイプ切り替え", "type <キャラID> <タイプID>", "Type", CommandCategory.System, 2, 2);
			this.RegisterCommandConfig("face", "キャラ表情", "face <キャラID> <表情ID> <タイプID:オプション> <オフセットX:オプション> <オフセットY:オプション>", "Face", CommandCategory.System, 2, 5);
			this.RegisterCommandConfig("focus", "キャラを明るく表示", "focus <キャラID>", "Focus", CommandCategory.System, 1, 1);
			this.RegisterCommandConfig("background", "背景設定", "background <背景ID もしくは カードID>", "Background", CommandCategory.System, 1, 1);
			this.RegisterCommandConfig("print", "テキスト表示", "pring <名前> <テキスト>", "Print", CommandCategory.System, 2, 2);
			this.RegisterCommandConfig("tag", "タグ設定", "tag <タグID>", "Tag", CommandCategory.System, 1, 1);
			this.RegisterCommandConfig("goto", "指定タグの位置に移動", "goto <飛ばしたいタグID>", "Goto", CommandCategory.System, 1, 1);
			this.RegisterCommandConfig("bgm", "BGM指定", "bgm <キュー名>", "Bgm", CommandCategory.System, 1, 2);
			this.RegisterCommandConfig("touch", "タッチ入力待ち", "touch", "Touch", CommandCategory.System, 0, 0);
			this.RegisterCommandConfig("choice", "選択肢追加", "choice <選択肢ラベル> <飛ばしたいタグID>", "Choice", CommandCategory.System, 2, 2);
			this.RegisterCommandConfig("vo", "ボイス再生", "vo <ボイスキュー名>", "Vo", CommandCategory.System, 1, 1);
			this.RegisterCommandConfig("wait", "指定時間待機", "wait <待ち時間>", "Wait", CommandCategory.System, 1, 1);
			this.RegisterCommandConfig("in_L", "左からスライドイン", "in_L <キャラID> <フレーム数:オプション> <フェードフラグ:オプション>", "InL", CommandCategory.Motion, 1, 3);
			this.RegisterCommandConfig("in_R", "右からスライドイン>", "in_R <キャラID> <フレーム数:オプション> <フェードフラグ:オプション>", "InR", CommandCategory.Motion, 1, 3);
			this.RegisterCommandConfig("out_L", "左へスライドアウト", "out_L <キャラID> <フレーム数:オプション> <フェードフラグ:オプション>", "OutL", CommandCategory.Motion, 1, 3);
			this.RegisterCommandConfig("out_R", "右へスライドアウト", "out_R <キャラID> <フレーム数:オプション> <フェードフラグ:オプション>", "OutR", CommandCategory.Motion, 1, 3);
			this.RegisterCommandConfig("fadein", "フェードイン>", "fadein <キャラID> <フレーム数:オプション>", "Fadein", CommandCategory.Motion, 1, 2);
			this.RegisterCommandConfig("fadeout", "フェードアウト", "fadeout <キャラID> <フレーム数:オプション>", "Fadeout", CommandCategory.Motion, 0, 2);
			this.RegisterCommandConfig("in_float", "フロートイン", "in_float <キャラID> <フレーム数:オプション>", "InFloat", CommandCategory.Motion, 1, 2);
			this.RegisterCommandConfig("out_float", "フロートアウト", "out_float <キャラID> <フレーム数:オプション>", "OutFloat", CommandCategory.Motion, 1, 2);
			this.RegisterCommandConfig("jump", "ジャンプ", "jump <キャラID> <回数:オプション>", "Jump", CommandCategory.Motion, 1, 2);
			this.RegisterCommandConfig("shake", "震える", "shake <キャラID> <回数:オプション>", "Shake", CommandCategory.Motion, 1, 2);
			this.RegisterCommandConfig("pop", "軽く弾む", "pop <キャラID> <回数:オプション>", "Pop", CommandCategory.Motion, 1, 2);
			this.RegisterCommandConfig("nod", "沈む", "nod <キャラID> <回数:オプション>", "Nod", CommandCategory.Motion, 1, 2);
			this.RegisterCommandConfig("question_right", "はてな\u3000右向き", "question_right <キャラID>", "QuestionRight", CommandCategory.Motion, 1, 1);
			this.RegisterCommandConfig("question_left", "はてな\u3000左向き", "question_left <キャラID>", "QuestionLeft", CommandCategory.Motion, 1, 1);
			this.RegisterCommandConfig("se", "SE再生", "se <キューシート> <キュー名>", "Se", CommandCategory.System, 1, 2);
			this.RegisterCommandConfig("black_out", "暗転", "black_out <アルファ:オプション> <時間:オプション>", "BlackOut", CommandCategory.System, 0, 2);
			this.RegisterCommandConfig("black_in", "暗転からの復帰", "black_in", "BlackIn", CommandCategory.System, 0, 2);
			this.RegisterCommandConfig("white_out", "ホワイトアウト", "white_out <アルファ:オプション> <時間:オプション>", "WhiteOut", CommandCategory.System, 0, 2);
			this.RegisterCommandConfig("white_in", "ホワイトアウトから復帰", "white_in", "WhiteIn", CommandCategory.System, 0, 2);
			this.RegisterCommandConfig("transition", "場面転換演出", "transition <アニメ名:オプション>", "Transition", CommandCategory.System, 0, 1);
			this.RegisterCommandConfig("situation", "指定文字列を帯表示", "situation <場面名>", "Situation", CommandCategory.System, 1, 1);
			this.RegisterCommandConfig("color_fadein", "指定色フェードイン", "color_fadein <RGBコード> <透明度> <時間>", "ColorFadein", CommandCategory.System, 3, 3);
			this.RegisterCommandConfig("flash", "フラッシュ", "flash", "Flash", CommandCategory.System, 0, 0);
			this.RegisterCommandConfig("shake_text", "テキストウィンドウを揺らす", "shake_text", "ShakeText", CommandCategory.System, 0, 0);
			this.RegisterCommandConfig("text_size", "フォントサイズ指定", "text_size <フォントサイズ>", "TextSize", CommandCategory.System, 1, 1);
			this.RegisterCommandConfig("shake_screen", "画面全体揺らす", "shake_screen", "ShakeScreen", CommandCategory.System, 0, 0);
			this.RegisterCommandConfig("double", "テキストを重ねて表示", "double <名前> <テキスト> <オフセットX:オプション> <オフセットY:オプション>", "Double", CommandCategory.System, 2, 4);
			this.RegisterCommandConfig("flower_y", "黄色の花びら", "flower_y <キャラID> <オフセットX:オプション> <オフセットY:オプション>", "FlowerY", CommandCategory.Effect, 1, 3);
			this.RegisterCommandConfig("flower_r", "赤い花びら", "flower_r <キャラID> <オフセットX:オプション> <オフセットY:オプション>", "FlowerR", CommandCategory.Effect, 1, 3);
			this.RegisterCommandConfig("concent", "集中線", "concent <キャラID> <オフセットX:オプション> <オフセットY:オプション>", "Concent", CommandCategory.Effect, 1, 3);
			this.RegisterCommandConfig("find_l", "気付(左)", "find_l <キャラID> <オフセットX:オプション> <オフセットY:オプション>", "FindL", CommandCategory.Effect, 1, 3);
			this.RegisterCommandConfig("find_r", "気付き(右)", "find_r <キャラID> <オフセットX:オプション> <オフセットY:オプション>", "FindR", CommandCategory.Effect, 1, 3);
			this.RegisterCommandConfig("laugh_l", "笑い三本線(左)", "laugh_l <キャラID> <オフセットX:オプション> <オフセットY:オプション>", "LaughL", CommandCategory.Effect, 1, 3);
			this.RegisterCommandConfig("laugh_r", "笑い三本線(右)", "laugh_r <キャラID> <オフセットX:オプション> <オフセットY:オプション>", "LaughR", CommandCategory.Effect, 1, 3);
			this.RegisterCommandConfig("chord_l", "音符(左)", "chord_l <キャラID> <オフセットX:オプション> <オフセットY:オプション>", "ChordL", CommandCategory.Effect, 1, 3);
			this.RegisterCommandConfig("chord_r", "音符(右)", "chord_r <キャラID> <オフセットX:オプション> <オフセットY:オプション>", "ChordR", CommandCategory.Effect, 1, 3);
			this.RegisterCommandConfig("sweat_l", "汗(左)", "sweat_l <キャラID> <オフセットX:オプション> <オフセットY:オプション>", "SweatL", CommandCategory.Effect, 1, 3);
			this.RegisterCommandConfig("sweat_r", "汗(右)", "sweat_r <キャラID> <オフセットX:オプション> <オフセットY:オプション>", "SweatR", CommandCategory.Effect, 1, 3);
			this.RegisterCommandConfig("question_l", "はてな(左)", "question_l <キャラID> <オフセットX:オプション> <オフセットY:オプション>", "QuestionL", CommandCategory.Effect, 1, 3);
			this.RegisterCommandConfig("question_r", "はてな(右)", "question_r <キャラID> <オフセットX:オプション> <オフセットY:オプション>", "QuestionR", CommandCategory.Effect, 1, 3);
			this.RegisterCommandConfig("angry", "ぷんすか(左)", "angry <キャラID> <オフセットX:オプション> <オフセットY:オプション>", "Angry", CommandCategory.Effect, 1, 3);
			this.RegisterCommandConfig("drop_l", "汗2(左)", "drop_l <キャラID> <オフセットX:オプション> <オフセットY:オプション>", "DropL", CommandCategory.Effect, 1, 3);
			this.RegisterCommandConfig("drop_r", "汗2(右)", "drop_r <キャラID> <オフセットX:オプション> <オフセットY:オプション>", "DropR", CommandCategory.Effect, 1, 3);
			this.RegisterCommandConfig("live", "ライブ遷移", "live <楽曲ID> <難易度(debut:1から数字上がる毎に難易度アップ)> <メンバー1ID> <メンバー2ID> <メンバー3ID> <メンバー4ID> <メンバー5ID> <観客フラグ:オプション>", "Live", CommandCategory.System, 7, 8);
			this.RegisterCommandConfig("scale", "拡縮", "scale <キャラID> <拡縮率(等倍は1)>", "Scale", CommandCategory.Motion, 2, 2);
			this.RegisterCommandConfig("title_telop", "タイトルテロップ", "title_telop <ストーリーID>", "TitleTelop", CommandCategory.System, 1, 1);
			this.RegisterCommandConfig("window_visible", "テキストウィンドウの表示切り替え", "window_visible <\"true\" か \"false\">", "WindowVisible", CommandCategory.System, 1, 1);
			this.RegisterCommandConfig("log", "ログにだけ表示するテキスト", "log <キャラID> <名前> <テキスト> <ボイスID : オプション>", "Log", CommandCategory.System, 3, 4);
			this.RegisterCommandConfig("novoice", "ボイス再生しない", "novoice", "NoVoice", CommandCategory.System, 0, 0);
			this.RegisterCommandConfig("attract", "話している人", "attract <キャラID>", "Attract", CommandCategory.System, 1, 1);
			this.RegisterCommandConfig("change", "キャラ位置入れ替え", "change <位置1> <位置2> <移動フレーム数:オプション>", "Change", CommandCategory.Motion, 2, 3);
			this.RegisterCommandConfig("fadeout_all", "キャラとウィンドウを消す", "fadeout_all <フェード時間(フレーム)>", "FadeoutAll", CommandCategory.System, 0, 1);
		}

		private void RegisterCommandConfig(string name, string summary, string usage, string className, CommandCategory category, int minArgCount, int maxArgCount)
		{
			int count = this._commandConfigList.Count;
			CommandConfig item = default(CommandConfig);
			item.ID = count - 1;
			item.Name = name;
			item.Summary = summary;
			item.Usage = usage;
			item.ClassName = className;
			item.Category = category;
			item.MinArgCount = minArgCount;
			item.MaxArgCount = maxArgCount;
			this._commandConfigList.Add(item);
		}

		public int GetCommandID(ref string name)
		{
			int count = this._commandConfigList.Count;
			for (int i = 0; i < count; i++)
			{
				if (this._commandConfigList[i].Name.Equals(name))
				{
					return i;
				}
			}
			return -1;
		}

		public string GetCommandName(int id)
		{
			if (id >= this._commandConfigList.Count | id < 0)
			{
				return null;
			}
			return this._commandConfigList[id].Name;
		}

		public string GetCommandSummary(int id)
		{
			if (id >= this._commandConfigList.Count | id < 0)
			{
				return null;
			}
			return this._commandConfigList[id].Summary;
		}

		public string GetCommandUsage(int id)
		{
			if (id >= this._commandConfigList.Count | id < 0)
			{
				return null;
			}
			return this._commandConfigList[id].Usage;
		}

		public string GetCommandClassName(int id)
		{
			if (id >= this._commandConfigList.Count | id < 0)
			{
				return null;
			}
			return this._commandConfigList[id].ClassName;
		}

		public CommandCategory GetCommandCategory(int id)
		{
			if (id >= this._commandConfigList.Count | id < 0)
			{
				return CommandCategory.Non;
			}
			return this._commandConfigList[id].Category;
		}

		public int GetCommandMinArgCount(int id)
		{
			if (id >= this._commandConfigList.Count | id < 0)
			{
				return 0;
			}
			return this._commandConfigList[id].MinArgCount;
		}

		public int GetCommandMaxArgCount(int id)
		{
			if (id >= this._commandConfigList.Count | id < 0)
			{
				return 0;
			}
			return this._commandConfigList[id].MaxArgCount;
		}

		public int GetCommandConfigListCount()
		{
			return this._commandConfigList.Count;
		}

		public string GetSpacer()
		{
			return char.ToString(' ');
		}
	}
}
