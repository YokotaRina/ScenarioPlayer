using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Command;
using Enums;
using Model.Character;
using Model.Log;
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
        [SerializeField, Tooltip("選択肢グループ")] GameObject selectGroup;
        [SerializeField, Tooltip("選択肢ボタンベース")] Button selectButtonBase;
        [SerializeField, Tooltip("選択肢用キャラクター")] Image selectCharacter;

        [SerializeField, Tooltip("ボタングループ")] GameObject buttonGroup;
        public GameObject ButtonGroup => buttonGroup;

        [SerializeField, Tooltip("ログボタン")] Button logButton;
        [SerializeField, Tooltip("ログウィンドウ")] GameObject logWindow;
        [SerializeField, Tooltip("ログテキスト")] TextMeshProUGUI logMessage;

        [SerializeField, Tooltip("TOP")] private GameObject topObject = default;
        [SerializeField, Tooltip("ルビ表示用タグ生成器")] private RubyTagGenerator rubyTagGenerator = default;
        public RubyTagGenerator RubyTagGenerator => rubyTagGenerator;

        // ファイル名
        private string _fileName = default;
        // カウンター用
        private int _counter;
        // ジャンプ地点
        private string _jumpPoint;

        /// <summary>
        /// 選択肢ボタンの保持リスト
        /// </summary>
        private List<Button> _selectButtonList;

        /// <summary>
        /// キャラクターの保持リスト
        /// </summary>
        private List<Image> _characterImageList;

        /// <summary>
        /// コマンドリスト
        /// </summary>
        private List<CommandBase> _commandList;

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
        /// ログ保持用のリスト
        /// </summary>
        private List<LogInfo> _logInfoList;

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
            SelectJump, // 選択肢のジャンプ
            Jump, // ジャンプ
            AllDone, // 全終了

            OpenLog, // ログ表示
            CloseLog, // ログ非表示
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
            _characterImageList = new List<Image>();
            _counter = 0;
            _jumpPoint = String.Empty;
            _commandList = new CommandRepository().GetCommandList(_fileName);
            _logInfoList = new List<LogInfo>();

            var characterRepository = new CharacterRepository();
            _characterList = characterRepository.GetCharacterList();
            _positionList = characterRepository.GetPositionList();

            var resourceRepository = new ResourceRepository();
            _backGroundList = resourceRepository.GetBackGroundList();
            _voiceList = resourceRepository.GetVoiceList();
            _bgmList = resourceRepository.GetBgmList();

            logButton.onClick.AddListener(() =>
            {
                _state.SetState((int) State.OpenLog);
            });
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
                    _currentCommand = _commandList[_counter];
                    _currentCommand.Start(this);

                    _counter++;
                    _state.SetState((int) State.CommandProcess);
                    break;
                case State.CommandProcess:
                    if (_currentCommand.AdvCommandType == AdvCommandType.Text)
                    {
                        // タップ判定
                        if (Input.GetMouseButtonUp(0))
                        {
                            if (_commandList.Count <= _counter)
                            {
                                _state.SetState((int) State.AllDone);
                            }
                            else
                            {
                                _state.SetState((int) State.EndCommand);
                            }
                        }
                    }
                    else if (_currentCommand.AdvCommandType == AdvCommandType.Select)
                    {
                        _state.SetState((int) State.SelectWait);
                    }
                    else if (_currentCommand.AdvCommandType == AdvCommandType.Jump)
                    {
                        _state.SetState((int) State.Jump);
                    }
                    else
                    {
                        _state.SetState((int) State.DoCommand);
                    }
                    break;
                case State.EndCommand:
                    _currentCommand.End();
                    _state.SetState((int) State.DoCommand);
                    break;
                case State.SelectJump:
                    // キャラクターの非表示
                    selectCharacter.gameObject.SetActive(false);
                    foreach (var characterImage in _characterImageList)
                    {
                        characterImage.gameObject.SetActive(true);
                    }

                    // ボタンの破棄
                    foreach (var selectButton in _selectButtonList)
                    {
                        Destroy(selectButton.gameObject);
                    }

                    _selectButtonList.Clear();
                    selectGroup.SetActive(false);

                    // 指定のコマンドまでジャンプ
                    var targetCommand = _commandList
                        .Select((command, i) => new {Content = command, Index = i})
                        .Where(x => x.Content.AdvCommandType == AdvCommandType.SelectPoint)
                        .FirstOrDefault(x => (x.Content as SelectPointCommand)?.Id == _jumpPoint);

                    if (targetCommand != null)
                    {
                        _counter = targetCommand.Index;
                        _state.SetState((int) State.DoCommand);
                    }
                    else
                    {
                        _state.SetState((int) State.DoCommand);
                    }
                    break;
                case State.Jump:
                    // 指定のコマンドまでジャンプ
                    targetCommand = _commandList
                        .Select((command, i) => new {Content = command, Index = i})
                        .Where(x => x.Content.AdvCommandType == AdvCommandType.JumpPoint)
                        .FirstOrDefault(x => (x.Content as JumpPointCommand)?.Id == _jumpPoint);

                    if (targetCommand != null)
                    {
                        _counter = targetCommand.Index;
                        _state.SetState((int) State.DoCommand);
                    }
                    else
                    {
                        _state.SetState((int) State.DoCommand);
                    }
                    break;
                case State.AllDone:
                    this.ReturnTop();
                    break;
                case State.OpenLog:
                    logWindow.SetActive(true);
                    var text = string.Empty;
                    foreach (var logInfo in _logInfoList)
                    {
                        if (string.IsNullOrEmpty(logInfo.Name))
                        {
                            text += $" {logInfo.Text}\n\n";
                        }
                        else
                        {
                            text += $"{logInfo.Name}\n";
                            text += $" {logInfo.Text}\n\n";
                        }
                    }
                    logMessage.text = text;
                    _state.SetState((int) State.CloseLog);
                    break;
                case State.CloseLog:
                    // タップ判定
                    if (Input.GetMouseButtonUp(0))
                    {
                        logWindow.SetActive(false);
                        _state.SetState((int) State.CommandProcess);
                    }
                    break;
            }
            _state.Update(Time.unscaledDeltaTime);
        }

        /// <summary>
        /// メッセージテキストを更新
        /// </summary>
        public void UpdateMessageText(TextCommand textCommand)
        {
            var message = textCommand.GetMessageText();
            var name = textCommand.Name;

            messageText.text = message;

            if (string.IsNullOrEmpty(name))
            {
                namePlate.SetActive(false);
            }
            else
            {
                namePlate.SetActive(true);
                nameText.text = name;
            }

            // ログの保持
            _logInfoList.Add(new LogInfo(name, message));
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
            _characterImageList.Add(character);
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
        /// 選択肢表示
        /// </summary>
        public void DisplaySelectGroup(SelectCommand selectCommand)
        {
            // キャラクターの非表示
            foreach (var characterImage in _characterImageList)
            {
                characterImage.gameObject.SetActive(false);
            }

            // 選択肢の生成
            _selectButtonList = new List<Button>();
            selectGroup.SetActive(true);
            foreach (var choiceWord in selectCommand.ChoiceWordList)
            {
                var button = Instantiate(selectButtonBase, selectButtonBase.transform.parent.transform);
                _selectButtonList.Add(button);
                button.gameObject.SetActive(true);
                var buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = choiceWord.Item1;

                button.onClick.AddListener(() =>
                {
                    _jumpPoint = choiceWord.Item2;
                    _state.SetState((int)State.SelectJump);

                    // ログの保持
                    _logInfoList.Add(new LogInfo("選択肢", buttonText.text));
                });
            }

            // メッセージテキスト表示
            var text = selectCommand.Text;
            messageText.text = text;

            // キャラクター表示
            var characterId = selectCommand.CharacterId;
            if (!string.IsNullOrEmpty(characterId))
            {
                var characterInfo = _characterList.FirstOrDefault(x => x.Id == characterId);
                if (characterInfo == null) return;
                selectCharacter.gameObject.SetActive(true);

                var pattern = selectCommand.FacePattern;
                var resource = string.Empty;
                var resourceDictionary = characterInfo.ResourceDictionary;
                if (resourceDictionary.ContainsKey(pattern))
                {
                    resource = resourceDictionary[pattern];
                }

                if (string.IsNullOrEmpty(resource)) return;

                var path = $"Character/{characterId}/{resource}";
                var fullPath = $"Assets/Resources/{path}.png";
                if (!File.Exists(fullPath)) return;
                
                Sprite image = Resources.Load<Sprite>(path);

                selectCharacter.gameObject.SetActive(true);
                selectCharacter.sprite = image;
            }

            // ボイス再生
            var voiceId = selectCommand.VoiceId;
            if (!string.IsNullOrEmpty(voiceId))
            {
                var voiceInfo = _voiceList.FirstOrDefault(x => x.Id == voiceId);
                if (voiceInfo == null) return;

                var path = $"Voice/{voiceInfo.Resource}";
                var fullPath = $"Assets/Resources/{path}.mp3";
                if (!File.Exists(fullPath)) return;

                AudioClip voice = Resources.Load<AudioClip>(path);
                voicePlayer.clip = voice;
                voicePlayer.Play();    
            }

            if (!string.IsNullOrEmpty(text))
            {
                // ログの保持
                _logInfoList.Add(new LogInfo(string.Empty, text));   
            }
        }

        /// <summary>
        /// ジャンプ地点設定
        /// </summary>
        public void SetJumpPoint(JumpCommand jumpCommand)
        {
            _jumpPoint = jumpCommand.JumpPointId;
        }

        /// <summary>
        /// Topへ戻る押下時
        /// </summary>
        private void ReturnTop()
        {
            // 画面切り替え
            this.gameObject.SetActive(false);
            buttonGroup.SetActive(false);
            topObject.SetActive(true);

            // キャラクター保持リストの破棄
            foreach (var characterImage in _characterImageList)
            {
                Destroy(characterImage.gameObject);
            }
            _characterImageList.Clear();
        }

        /// <summary>
        /// Destroy
        /// </summary>
        private void OnDestroy()
        {
            if (_commandList != null)
            {
                _commandList.Clear();
                _commandList = null;
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

            if (_characterImageList != null)
            {
                foreach (var characterImage in _characterImageList)
                {
                    Destroy(characterImage.gameObject);
                }

                _characterImageList.Clear();
                _characterImageList = null;
            }

            if (_selectButtonList != null)
            {
                foreach (var selectButton in _selectButtonList)
                {
                    Destroy(selectButton.gameObject);
                }

                _selectButtonList.Clear();
                _selectButtonList = null;
            }

            if (_logInfoList != null)
            {
                _logInfoList.Clear();
                _logInfoList = null;
            }

            _state = null;
        }
    }
}
