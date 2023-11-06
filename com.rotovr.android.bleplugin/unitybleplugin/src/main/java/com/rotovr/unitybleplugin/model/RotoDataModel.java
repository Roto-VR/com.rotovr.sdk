package com.rotovr.unitybleplugin.model;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.Serializable;

public class RotoDataModel implements Serializable {

    public RotoDataModel() {
        Mode = "";
        Angle = 0;
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

        if (data[5] > 0) {
            Angle = data[6] + 256;
        } else {
            Angle = data[6];
        }

    }

    public String Mode;
    public int Angle;


    public boolean Compare(RotoDataModel model) {
        return Mode == model.Mode && Angle == model.Angle;
    }

    public String toJson() {
        JSONObject obj = new JSONObject();
        try {
            obj.put("Mode", Mode);
            obj.put("Angle", Angle);

            return obj.toString();
        } catch (JSONException e) {
            e.printStackTrace();
        }

        return obj.toString();
    }
}
