using System;
using System.Collections;
using System.Collections.Generic;

public class ConfigurationManager : IController
{
    private Dictionary<string, Setting> _settings = new Dictionary<string, Setting>();

    public void AddSetting(Setting s)
    {
        if (_settings.ContainsKey(s.Name))
        {
            //if it already exists, we just update the value
            _settings[s.Name].Value = s.Value;
        }
        _settings.Add(s.Name, s);
    }

    public void AddSetting(string name, object value, Type t)
    {
        AddSetting(new Setting()
        {
            Name = name,
            Value = value,
            ValueType = t
        });
    }

    public void AddSetting<T>(string name, object value)
    {
        AddSetting(new Setting()
        {
            Name = name,
            Value = value,
            ValueType = typeof(T)
        });
    }

    public object GetSettingValue(string name)
    {
        return _settings[name].Value;
    }

    public bool TryGetSettingsValue(string name, out object value)
    {
        value = null;

        if (!_settings.ContainsKey(name))
        {
            return false;
        }
        else
        {
            value = _settings[name].Value;
            return true;
        }
    }

    public Setting GetSetting(string name)
    {
        return _settings[name];
    }

    public bool TryGetSetting(string name, out Setting setting)
    {
        setting = null;

        if (!_settings.ContainsKey(name))
        {
            return false;
        }
        else
        {
            setting = _settings[name];
            return true;
        }
    }

    public bool SettingExists(string name)
    {
        return _settings.ContainsKey(name);
    }

    public void UpdateSetting(string name, object value)
    {
        _settings[name].Value = value;
    }

    public void UpdateSetting(string name, object value, Type valueType)
    {
        _settings[name].Value = value;
        _settings[name].ValueType = valueType;
    }

    /// <summary>
    /// Attempts to update a setting value.  Returns false if the setting does not exist.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryUpdateSetting(string name, object value)
    {
        bool settingExists = false;
        if (_settings.ContainsKey(name))
        {
            UpdateSetting(name, value);
            settingExists = true;
        }
        return settingExists;
    }

    /// <summary>
    /// Attempts to update a setting value and value type.  Returns false if the setting does not exist.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="valueType"></param>
    /// <returns></returns>
    public bool TryUpdateSetting(string name, object value, Type valueType)
    {
        bool settingExists = false;
        if (_settings.ContainsKey(name))
        {
            UpdateSetting(name, value, valueType);
            settingExists = true;
        }
        return settingExists;
    }

    public IEnumerable<Setting> AllSettings()
    {
        foreach (KeyValuePair<string,Setting> setting in _settings)
        {
            yield return setting.Value;
        }
    }

    public void Cleanup()
    {
        
    }
}

public class Setting
{
    public string Name { get; set; }
    public object Value { get; set; }
    public Type ValueType { get; set; }
}

