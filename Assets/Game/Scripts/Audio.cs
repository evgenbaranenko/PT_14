using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Audio
{
    /// <summary>
    /// ³��������� ����� � ������

    /// </summary>
    /// <param name="clipName">��'� �����</param>
    public void PlaySound(string clipName)
    {
        SourceSFX.PlayOneShot(GetSound(clipName), SfxVolume);
    }

    /// <summary>
    /// ³��������� ����� � ������ � ���������� ��������
    /// </summary>
    /// <param name="clipName">��'� �����</param>
    public void PlaySoundRandomPitch(string clipName)
    {
        SourceRandomPitchSFX.pitch = Random.Range(0.7f, 1.3f);
        SourceRandomPitchSFX.PlayOneShot(GetSound(clipName),
        SfxVolume);
    }
    /// <summary>
    /// ³��������� ������
    /// </summary>
    /// <param name="menu">��� ��������� ����?</param>
    public void PlayMusic(bool menu)
    {
        if (menu)
            SourceMusic.clip = menuMusic;
        else
            SourceMusic.clip = gameMusic;

        SourceMusic.volume = MusicVolume;

        SourceMusic.loop = true;

        SourceMusic.Play();
    }


    /// <summary>
    /// ����� ����� � �����
    /// </summary>
    /// <param name="clipName">��'� �����</param>
    /// <returns>����. ���� ���� �� ��������, ����������� �������� ����� default
    ///Clip</returns>
    private AudioClip GetSound(string clipName)
    {
        for (var i = 0; i < sounds.Length; i++)
            if (sounds[i].name == clipName) return sounds[i];

        Debug.LogError("Can not find clip " + clipName);

        return defaultClip;
    }

    #region Private_Variables

    //��������� �� ������� ����� ��� ���������� �����
    private AudioSource sourceSFX;
    //��������� �� ������� ����� ��� ���������� ������ private
    private AudioSource sourceMusic;
    //��������� �� ������� ����� ��� ���������� �����
    //� ���������� ��������
    private AudioSource sourceRandomPitchSFX;

    //������� ������
    private float musicVolume = 1f;
    public float MusicVolume
    {
        get
        {
            return musicVolume;
        }
        set
        {
            musicVolume = value;

            SourceMusic.volume = musicVolume;

            DataStore.SaveOptions();
        }
    }

    //������� �����
    private float sfxVolume = 1f;

    //����� �����
    [SerializeField] private AudioClip[] sounds;

    //���� �� ������������� �� �������, ���� � ����� ������� ����������
    [SerializeField] private AudioClip defaultClip;

    //������ ��� ��������� ����
    [SerializeField] private AudioClip menuMusic;

    //������ ��� ��� �� �����

    [SerializeField] private AudioClip gameMusic;

    #endregion

    #region Public_Properties
    public AudioSource SourceSFX
    {
        get { return sourceSFX; }
        set { sourceSFX = value; }
    }

    public AudioSource SourceMusic
    {
        get { return sourceMusic; }
        set { sourceMusic = value; }
    }

    public AudioSource SourceRandomPitchSFX
    {
        get { return sourceRandomPitchSFX; }
        set { sourceRandomPitchSFX = value; }
    }


    public float SfxVolume
    {
        get { return sfxVolume; }
        set
        {

            sfxVolume = value;

            SourceSFX.volume = sfxVolume;

            SourceRandomPitchSFX.volume = sfxVolume;

            DataStore.SaveOptions();

        }
    }

    #endregion
}