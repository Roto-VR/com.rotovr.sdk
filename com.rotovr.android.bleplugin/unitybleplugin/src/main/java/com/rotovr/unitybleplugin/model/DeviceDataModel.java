package com.rotovr.unitybleplugin.model;

import org.json.JSONException;
import org.json.JSONObject;
import java.io.Serializable;


public class DeviceDataModel implements Serializable {

    public DeviceDataModel(String name, String address) {
        Name = name;
        Address = address;
    }

    public String Name;
    public String Address;

    public String toJson() {
        JSONObject obj = new JSONObject();
        try {
            obj.put("Name", Name);
            obj.put("Address", Address);

            return obj.toString();
        } catch (JSONException e) {
            e.printStackTrace();
        }

        return obj.toString();
    }
}
