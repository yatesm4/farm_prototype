using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Farm_Prototype.Content
{
    /// <summary>
    /// Class for storing dynamic content
    /// Uses t for generic type, types range from Texture2D to SoundEffect etc
    /// Uses Id to identify specific content data within lists
    /// Stores path for loading content from contentmanager
    /// Data is the actual content itself, loaded from the path. T is the generic type for the data
    /// </summary>
    /// <typeparam name="T">Generic Content Type</typeparam>
    public class ContentData<T>
    {
        public ContentData(int id, string path, ContentManager content)
        {
            Id = id;
            Path = path;
            Data = content.Load<T>(path);
        }

        public int Id { get; set; } = 0;
        public string Path { get; set; } = string.Empty;
        public T Data { get; set; } = default(T);

    }

    /// <summary>
    /// Content management class to store ContentData lists.
    /// Loads all textures needed within the game and can return specified content from lists
    /// </summary>
    public class GameContent
    {
        // where to load content from
        private ContentManager _content;

        // private lists to store contentdata
        private List<ContentData<Texture2D>> _uiTexturesList { get; set; } = new List<ContentData<Texture2D>>();
        private List<ContentData<SpriteFont>> _fontsList { get; set; } = new List<ContentData<SpriteFont>>();
        private List<ContentData<Texture2D>> _tileTexturesList { get; set; } = new List<ContentData<Texture2D>>();
        private List<ContentData<Texture2D>> _headTexturesList { get; set; } = new List<ContentData<Texture2D>>();
        private List<ContentData<Texture2D>> _bodyTexturesList { get; set; } = new List<ContentData<Texture2D>>();
        private List<ContentData<Texture2D>> _npcTexturesList { get; set; } = new List<ContentData<Texture2D>>();
        private List<ContentData<SoundEffect>> _soundEffectsList { get; set; } = new List<ContentData<SoundEffect>>();

        // list accessors
        public List<ContentData<Texture2D>> UiTextures
        {
            get { return _uiTexturesList; }
            set { _uiTexturesList = value; }
        }
        public List<ContentData<SpriteFont>> Fonts
        {
            get { return _fontsList; }
            set { _fontsList = value; }
        }
        public List<ContentData<Texture2D>> TileTextures
        {
            get { return _tileTexturesList; }
            set { _tileTexturesList = value; }
        }
        public List<ContentData<Texture2D>> HeadTextures
        {
            get { return _headTexturesList; }
            set { _headTexturesList = value; }
        }
        public List<ContentData<Texture2D>> BodyTextures
        {
            get { return _bodyTexturesList; }
            set { _bodyTexturesList = value; }
        }
        public List<ContentData<Texture2D>> NpcTextures
        {
            get { return _npcTexturesList; }
            set { _npcTexturesList = value; }
        }
        public List<ContentData<SoundEffect>> SoundEffects
        {
            get { return _soundEffectsList; }
            set { _soundEffectsList = value; }
        }

        // get specific content data based on an id from a content type category
        public Texture2D GetUiTexture(int id)
        {
            return (from a in UiTextures
                    where a.Id.Equals(id)
                    select a.Data).SingleOrDefault<Texture2D>();
        }
        public SpriteFont GetFont(int id)
        {
            return (from a in Fonts
                    where a.Id.Equals(id)
                    select a.Data).SingleOrDefault<SpriteFont>();
        }
        public Texture2D GetTileTexture(int id)
        {
            return (from a in TileTextures
                    where a.Id.Equals(id)
                    select a.Data).SingleOrDefault<Texture2D>();
        }
        public Texture2D GetHeadTexture(int id)
        {
            return (from a in HeadTextures
                    where a.Id.Equals(id)
                    select a.Data).SingleOrDefault<Texture2D>();
        }
        public Texture2D GetBodyTexture(int id)
        {
            return (from a in BodyTextures
                    where a.Id.Equals(id)
                    select a.Data).SingleOrDefault<Texture2D>();
        }
        public Texture2D GetNpcTexture(int id)
        {
            return (from a in NpcTextures
                    where a.Id.Equals(id)
                    select a.Data).SingleOrDefault<Texture2D>();
        }
        public SoundEffect GetSoundEffect(int id)
        {
            return (from a in SoundEffects
                    where a.Id.Equals(id)
                    select a.Data).SingleOrDefault<SoundEffect>();
        }

        // constructor
        // takes a contentmanager as an arg
        // loads all textures within the game
        // TODO
        //  Pass id to only load specific textures (useful for menu state so it doesnt load game textures when it doesnt need to
        public GameContent(ContentManager content)
        {
            _content = content;

            LoadUITextures();
            LoadFonts();
            LoadTileTextures();
            LoadHeadTextures();
            LoadBodyTextures();
            LoadNpcTextures();
            LoadSoundEffects();
        }

        // load ui textures 
        public void LoadUITextures()
        {
            var i = 1;
            UiTextures.Add(new ContentData<Texture2D>(i++, "Sprites/UI/UI_Button", _content));
            UiTextures.Add(new ContentData<Texture2D>(i++, "Sprites/UI/UI_SpeechBubble", _content));
            UiTextures.Add(new ContentData<Texture2D>(i++, "Sprites/UI/BubbleIcons/QueWeed", _content));
            UiTextures.Add(new ContentData<Texture2D>(i++, "Sprites/UI/BubbleIcons/QueMoney", _content));
            UiTextures.Add(new ContentData<Texture2D>(i++, "Sprites/UI/UI_DownArrow", _content));
        }

        public void LoadFonts()
        {
            // total: 1
            var i = 1;
            Fonts.Add(new ContentData<SpriteFont>(i++, "Fonts/Font_01", _content));
        }

        // load tile/tileset textures
        public void LoadTileTextures()
        {
            // total: 12
            var i = 1;
            TileTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Environment/Ground_Glow", _content));
            TileTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Environment/Ground_Grass", _content));
            TileTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Environment/Ground_Tree", _content));
            TileTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Environment/Ground_Road", _content));
            TileTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Environment/Ground_Cement", _content));
            TileTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Environment/Ground_Grass_Bench", _content));
            TileTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Environment/Ground_Road_Left", _content));
            TileTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Environment/Ground_Road_Right", _content));
            TileTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Environment/Structures/Room_01", _content));
            TileTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Environment/Structures/Room_01_Floor", _content));
            TileTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Environment/Structures/Room_02", _content));
            TileTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Environment/Structures/Room_03", _content));
        }

        // load player textures
        public void LoadBodyTextures()
        {
            // total: 5
            var i = 1;
            BodyTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Characters/Body/02_SouthWest", _content));
            BodyTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Characters/Body/02_SouthEast", _content));
            BodyTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Characters/Body/02_NorthWest", _content));
            BodyTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Characters/Body/02_NorthEast", _content));

        }
        public void LoadHeadTextures()
        {
            // total: 5
            var i = 1;
            HeadTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Characters/Head/01", _content));
            HeadTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Characters/Head/02", _content));
            HeadTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Characters/Head/03", _content));
        }

        public void LoadNpcTextures()
        {
            // total: 1
            var i = 1;
            NpcTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Characters/NPCs/01", _content));
        }

        // load sound effects
        public void LoadSoundEffects()
        {
            // total: 1
            var i = 1;
            SoundEffects.Add(new ContentData<SoundEffect>(i++, "Sounds/Effects/footstep", _content));
        }
    }
}
