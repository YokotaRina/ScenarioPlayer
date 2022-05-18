using System.Collections.Generic;
using System.IO;
using System.Linq;
using Command;
using Enums;
using Model.Character;
using Model.Sound;
using Model.Texture;
using Repository;
using Ruby;
using Script.State;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller
{
    /// <summary>
    /// Advのコントローラー
    /// </summary>
    public class AdvController : MonoBehaviour
    {
        [SerializeField, Tooltip("メッセージテキスト")] private TextMeshProUGUI messageText = default;
        [SerializeField, Tooltip("名前テキスト")] private TextMeshProUGUI nameText = default;
        [SerializeField, Tooltip("名前欄")] private GameObject namePlate = default;
        [SerializeField, Tooltip("背景")] private Image backGround = default;
        [SerializeField, Tooltip("キャラクターベース")] private Image characterBase = default;
        [SerializeField, Tooltip("ボイス用AudioSource")] AudioSource voicePlayer;
        [SerializeField, Tooltip("BGM用AudioSource")] AudioSource bgmPlayer;

        [SerializeField, Tooltip("TOP")] private GameObject topObject = default;
        [SerializeField, Tooltip("ルビ表示用タグ生成器")] private RubyTagGenerator rubyTagGenerator = default;
        public RubyTagGenerator RubyTagGenerator => rubyTagGenerator;

        // ファイル名
        private string _fileName = default;

        /// <summary>
        /// コマンドリスト
        /// </summary>
        private Queue<CommandBase> _commandQueue;

        /// <summary>
        /// キャラクターリスト
        /// </summary>
        private List<CharacterBase> _characterList;

        /// <summary>
        /// キャラクター表示位置リスト
        /// </summary>
        private List<PositionBase> _positionList;

        /// <summary>
        /// 背景情報リスト
        /// </summary>
        private List<BackGroundBase> _backGroundList;

        /// <summary>
        /// ボイス情報リスト
        /// </summary>
        private List<VoiceBase> _voiceList;

        /// <summary>
        /// BGM情報リスト
        /// </summary>
        private List<BgmBase> _bgmList;

        /// <summary>
        /// 現在実行中のコマンド
        /// </summary>
        private CommandBase _currentCommand;

        /// <summary>
        /// ステートの種類
        /// </summary>
        private enum State
        {
            SelectWait, // 入力待ち

            Initialize, // 初期化
            DoCommand,  // コマンド実行
            CommandProcess,  // コマンド実行中
            EndCommand, // コマンド実行終了
            AllDone, // 前終了
        }

        /// <summary>
        /// ステート
        /// </summary>
        private StateManager _state;

        /// <summary>
        /// SetUp
        /// </summary>
        public void SetUp(string fileName)
        {
            _fileName = fileName;

            _state = new StateManager();
            _state.SetState((int)State.Initialize);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialize()
        {
            _commandQueue = new Queue<CommandBase>(new CommandRepository().GetCommandList(_fileName));

            var characterRepository = new CharacterRepository();
            _characterList = characterRepository.GetCharacterList();
            _positionList = characterRepository.GetPositionList();

            var resourceRepository = new ResourceRepository();
            _backGroundList = resourceRepository.GetBackGroundList();
            _voiceList = resourceRepository.GetVoiceList();
            _bgmList = resourceRepository.GetBgmList();
        }

        /// <summary>
        /// Update
        /// </summary>
        void Update()
        {
            if (_state == null) return;

            switch ((State) _state.GetState())
            {
                case State.SelectWait:
                    // 何もしない
                    break;
                case State.Initialize:
                    if (_state.IsOnEntry())
                    {
                        this.Initialize();
                        _state.SetState((int) State.DoCommand);
                    }

                    break;
                case State.DoCommand:
                    // コマンド実行
                    _currentCommand = _commandQueue.Dequeue();
                    _currentCommand.Start(this);

                    _state.SetState((int) State.CommandProcess);
                    break;
                case State.CommandProcess:
                    // 何もしない
                    // タップ判定
                    if (_currentCommand.AdvCommandType == AdvCommandType.Text)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            if (_commandQueue.Count == 0)
                            {
                                _state.SetState((int) State.AllDone);
                            }
                            else
                            {
                                _state.SetState((int) State.EndCommand);
                            }
                        }
                    }
                    else
                    {
                        _state.SetState((int)State.DoCommand);
                    }
                    break;
                case State.EndCommand:
                    _currentCommand.End();
                    _state.SetState((int)State.DoCommand);
                    break;
                case State.AllDone:
                    this.ReturnTop();
                    break;
            }
            _state.Update(Time.unscaledDeltaTime);
        }

        /// <summary>
        /// メッセージテキストを更新
        /// </summary>
        public void UpdateMessageText(TextCommand textCommand)
        {
            messageText.text = textCommand.GetMessageText();
            if (string.IsNullOrEmpty(textCommand.Name))
            {
                namePlate.SetActive(false);
            }
            else
            {
                namePlate.SetActive(true);
                nameText.text = textCommand.Name;
            }
        }

        /// <summary>
        /// 背景を更新
        /// </summary>
        public void UpdateBackGround(BackGroundCommand backGroundCommand)
        {
            var backGroundInfo = _backGroundList.FirstOrDefault(x => x.Id == backGroundCommand.Id);
            if (backGroundInfo == null) return;

            var path = $"BackGround/{backGroundInfo.Resource}";
            var fullPath = $"Assets/Resources/{path}.png";
            if (!File.Exists(fullPath)) return;

            Sprite image = Resources.Load<Sprite>(path);
            backGround.sprite = image;
        }

        /// <summary>
        /// ボイス再生
        /// </summary>
        public void PlayVoice(VoiceCommand voiceCommand)
        {
            var voiceInfo = _voiceList.FirstOrDefault(x => x.Id == voiceCommand.Id);
            if (voiceInfo == null) return;

            var path = $"Voice/{voiceInfo.Resource}";
            var fullPath = $"Assets/Resources/{path}.mp3";
            if (!File.Exists(fullPath)) return;

            AudioClip voice = Resources.Load<AudioClip>(path);
            voicePlayer.clip = voice;
            voicePlayer.Play();
        }

        /// <summary>
        /// BGM再生
        /// </summary>
        public void PlayBgm(BgmCommand bgmCommand)
        {
            var bgmInfo = _bgmList.FirstOrDefault(x => x.Id == bgmCommand.Id);
            if (bgmInfo == null) return;

            var path = $"Bgm/{bgmInfo.Resource}";
            var fullPath = $"Assets/Resources/{path}.mp3";
            if (!File.Exists(fullPath)) return;

            AudioClip bgm = Resources.Load<AudioClip>(path);
            bgmPlayer.clip = bgm;
            bgmPlayer.loop = bgmInfo.IsLoop;
            bgmPlayer.Play();
        }

        /// <summary>
        /// キャラクター表示
        /// </summary>
        public void DisplayCharacter(CharacterCommand characterCommand)
        {
            var id = characterCommand.Id;
            var characterInfo = _characterList.FirstOrDefault(x => x.Id == id);
            var positionInfo = _positionList.FirstOrDefault(x => x.Id == characterCommand.PositionId);
            if (characterInfo == null || positionInfo == null) return;

            var pattern = characterCommand.FacePattern;
            var resource = string.Empty;
            var resourceDictionary = characterInfo.ResourceDictionary;
            if (resourceDictionary.ContainsKey(pattern))
            {
                resource = resourceDictionary[pattern];
            }

            if (string.IsNullOrEmpty(resource)) return;

            var path = $"Character/{id}/{resource}";
            var fullPath = $"Assets/Resources/{path}.png";
            if (!File.Exists(fullPath)) return;

            Sprite image = Resources.Load<Sprite>(path);

            var character = Instantiate(characterBase, characterBase.transform.parent.transform);
            character.gameObject.SetActive(true);
            character.sprite = image;

            var scale = characterInfo.Scale;
            character.transform.localScale = new Vector3(scale, scale);

            var canvas = character.GetComponent<Canvas>();
            canvas.sortingOrder = positionInfo.Order;

            Vector3 pos = character.transform.localPosition;
            pos.x = positionInfo.X;    // ワールド座標を基準にした、x座標を1に変更
            pos.y = positionInfo.Y;    // ワールド座標を基準にした、y座標を1に変更
            character.transform.localPosition = pos;
        }

        /// <summary>
        /// Topへ戻る押下時
        /// </summary>
        private void ReturnTop()
        {
            // 画面切り替え
            this.gameObject.SetActive(false);
            topObject.SetActive(true);
        }

        /// <summary>
        /// Destroy
        /// </summary>
        private void OnDestroy()
        {
            if (_commandQueue != null)
            {
                _commandQueue.Clear();
                _commandQueue = null;
            }

            if (_characterList != null)
            {
                _characterList.Clear();
                _characterList = null;
            }

            if (_positionList != null)
            {
                _positionList.Clear();
                _positionList = null;
            }

            if (_voiceList != null)
            {
                _voiceList.Clear();
                _voiceList = null;
            }

            if (_bgmList != null)
            {
                _bgmList.Clear();
                _bgmList = null;
            }

            _state = null;
        }
    }
}
