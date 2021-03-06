﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;

public class CSVManager : MonoBehaviour
{
    private List<string> _reportHeaders;
    private Dictionary<string, string> _dictionary;
    private string _rootPath;
    private string _path;

    private const string REPORT_SEPARATOR = ",";
    private const string REPORT_FILE_NAME = "Data.csv";

    private void Awake()
    {
        if (Application.isEditor)
        {
            _rootPath = "Assets/Resources/";
        }
        else
        {
            _rootPath = Application.persistentDataPath + "/";
        }
    }

    public void Initialize(object dataObject)
    {
        if (dataObject == null) return;
        
        var date = DateTime.Now.ToString("MM-dd-yyyy");

        _path = _rootPath + date + "_" + dataObject.GetType().Name + REPORT_FILE_NAME;
        
        InitializeHeaders(dataObject);
        InitializeDictionary();
    }

    public void UpdateData(object dataObject)
    {
        if (dataObject == null) return;
        
        var fields = dataObject.GetType().GetFields();
        foreach (var field in fields)
        {
            if (field.GetValue(dataObject) != null) AddData(field.Name, field.GetValue(dataObject).ToString());
        }
    }

    public void AppendReport()
    {
        if (_dictionary == null) return;
        
        var dictionaryValues = new List<string>();
        foreach (var value in _dictionary.Values)
        {
            dictionaryValues.Add(value);
        }
        
        AppendToReport(dictionaryValues);
        InitializeDictionary();
    }
    
    
    private void InitializeDictionary()
    {
        _dictionary = new Dictionary<string, string>();
        foreach (var header in _reportHeaders)
        {
            _dictionary.Add(header, null);
        }
    }
    
    private void InitializeHeaders(object dataObject)
    {
        _reportHeaders = new List<string>();
        var fields = dataObject.GetType().GetFields();
        foreach (var field in fields)
        {
            _reportHeaders.Add(field.Name);
        }
    }
    
    private void AddData(string header, string entry)
    {
        if (!_dictionary.ContainsKey(header)) return;

        _dictionary[header] = entry;
    }

    private void AppendToReport(IEnumerable<string> lineOfData)
    {
        VerifyFile();
        using (var streamWriter = File.AppendText(_path))
        {
            string finalString = "";
            foreach (var entry in lineOfData)
            {
                if (finalString != "")
                {
                    finalString += REPORT_SEPARATOR;
                }

                finalString += entry;
            }

            streamWriter.WriteLine(finalString);
        }
    }

    private void CreateReport()
    {
        using (var streamWriter = File.CreateText(_path))
        {
            var finalString = "";
            foreach (var header in _reportHeaders)
            {
                if (finalString != "")
                {
                    finalString += REPORT_SEPARATOR;
                }

                finalString += header;
            }

            streamWriter.WriteLine(finalString);
        }
    }

    private void VerifyFile()
    {
        string file = GetFilePath();
        if (!File.Exists(file))
        {
            CreateReport();
        }
    }

    private string GetFilePath()
    {
        return _path;
    }

    private void ClearData()
    {
        if (_dictionary == null) return;
        
        foreach (var key in _dictionary.Keys)
        {
            _dictionary[key] = null;
        }
    }
}