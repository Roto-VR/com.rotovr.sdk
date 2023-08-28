package com.rotovr.unitybleplugin.model;

import com.rotovr.unitybleplugin.MessageType;

import org.json.JSONException;
import org.json.JSONObject;

public class MessageModel {

    public MessageModel(MessageType messageType) {
        Command = messageType.name();
    }

    public MessageModel(MessageType messageType, String data) {
        Command = messageType.name();
        Data = data;
    }

    public String Command;
    public String Data;

    public String toJson() {
        JSONObject obj = new JSONObject();
        try {
            obj.put("Command", Command);
            obj.put("Data", Data);

            return obj.toString();
        } catch (JSONException e) {
            e.printStackTrace();
        }

        return obj.toString();
    }
}