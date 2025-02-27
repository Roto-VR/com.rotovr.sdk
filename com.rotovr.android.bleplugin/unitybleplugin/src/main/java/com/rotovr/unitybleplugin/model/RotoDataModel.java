package com.rotovr.unitybleplugin.model;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.Serializable;

public class RotoDataModel implements Serializable {

    public RotoDataModel() {
        Mode = "";
        Angle = 0;
        TargetCockpit = 140;
        MaxPower = 100;
    }

    public RotoDataModel(byte[] data) {

        switch (data[2]) {
            case 0:
                Mode = "IdleMode";
                break;
            case 1:
                Mode = "Calibration";
                break;
            case 2:
                Mode = "HeadTrack";
                break;
            case 3:
                Mode = "FreeMode";
                break;
            case 4:
                Mode = "CockpitMode";
                break;
            case 5:
                Mode = "Error";
                break;
        }

        switch (data[5]) {
            case 0:
                Angle = data[6] & 0xFF;
                break;
            case 1:
                int angle = data[6] & 0xFF;
                Angle = (angle + 256);
                break;

        }
        TargetCockpit = data[9] & 0xFF;
        MaxPower = data[12] & 0xFF;
    }

    public String Mode;
    public int Angle;
    public int TargetCockpit;
    public int MaxPower;


    public boolean Compare(RotoDataModel model) {
        return Mode == model.Mode && Angle == model.Angle && TargetCockpit == model.TargetCockpit && MaxPower == model.MaxPower;
    }

    public String toJson() {
        JSONObject obj = new JSONObject();
        try {
            obj.put("Mode", Mode);
            obj.put("Angle", Angle);
            obj.put("TargetCockpit", TargetCockpit);
            obj.put("MaxPower", MaxPower);

            return obj.toString();
        } catch (JSONException e) {
            e.printStackTrace();
        }

        return obj.toString();
    }
}
