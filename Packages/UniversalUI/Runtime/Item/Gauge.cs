using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
    [SerializeField]
    private Image fill;
    [SerializeField]
    private TMP_Text titleLabel;
    [SerializeField]
    private TMP_Text valueLabel;

    /// <summary>
    /// 表示フォーマット
    /// </summary>
    [SerializeField, Tooltip("{value}でvalueの値を表示、{max}でmaxの値を表示")]
    private string labelFormat = "{value}%";
    [SerializeField]
    private float max = 1f;
    [SerializeField]
    private float value = 0f;

    /// <summary>
    /// 一番小さい桁の切り捨て、四捨五入をどうするかの設定
    /// </summary>
    [SerializeField]
    private bool isFloor = true;

    /// <summary>
    /// 小数点以下の表示桁数
    /// </summary>
    [SerializeField, Range(0, 10)]
    private int decimalPlaces = 2;

    /// <summary>
    /// 表示する値を何倍にするか？
    /// max0で、この値が100なら0 ～ 100の表示
    /// </summary>
    [SerializeField]
    private int displayValueMagnification = 1;

    public string Title
    {
        set => titleLabel.SetText(value);
    }

    public float Value
    {
        get => value;
        set
        {
            this.value = Mathf.Min(value, max);
            fill.fillAmount = this.value / max;
            ApplyFormat();
        }
    }

    public void SetPercent(float percent)
    {
        max = 1f;
        Value = percent;
    }

    public void SetPercent(float value, float max)
    {
        SetPercent(value / max);
    }

    public void SetValues(float value, float max)
    {
        this.max = max;
        Value = value;
    }

    public void SetFormat(string format)
    {
        labelFormat = format;
        ApplyFormat();
    }

    public void SetDisplayValueMagnigication(int value)
        => displayValueMagnification = value;

    /// <summary>
    /// 表示用の設定を適用する
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    private float ApplyDisplaySetting(float original)
    {
        var value = original * displayValueMagnification;
        var pow = (int)Mathf.Pow(10, decimalPlaces);
        value = value * pow;
        value = isFloor ? Mathf.Floor(value) : Mathf.Round(value);
        value /= pow;
        return value;
    }

    /// <summary>
    /// 表示フォーマットを適用する
    /// </summary>
    private void ApplyFormat()
    {
        if (valueLabel == null) return;
        if (string.IsNullOrEmpty(labelFormat))
        {
            valueLabel.SetText((value * displayValueMagnification).ToString());
            return;
        }
        var text = labelFormat
            .Replace("{value}", ApplyDisplaySetting(value).ToString())
            .Replace("{max}", ApplyDisplaySetting(max).ToString());
        valueLabel.SetText(text);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying) return;
        value = Mathf.Clamp(value, 0f, max);
        if (fill != null)
            fill.fillAmount = value / max;
        ApplyFormat();
    }
#endif
}
