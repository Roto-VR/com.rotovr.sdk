package com.rotovr.unitybleplugin.utility;



import com.google.gson.Gson;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;

public class PluginUtility {
    public static byte[] ConvertObjectToByteArray(Object obj) {
        ByteArrayOutputStream boas = new ByteArrayOutputStream();
        try (ObjectOutputStream ois = new ObjectOutputStream(boas)) {
            ois.writeObject(obj);
            return boas.toByteArray();
        } catch (IOException ioe) {
            ioe.printStackTrace();
        }
        throw new RuntimeException();
    }

    public static Object ConvertByteArrayToObject(byte[] bytes) {

        try (ByteArrayInputStream bis = new ByteArrayInputStream(bytes);
             ObjectInputStream ois = new ObjectInputStream(bis)) {
            return ois.readObject();

        } catch (IOException | ClassNotFoundException ioe) {
            ioe.printStackTrace();
        }
        throw new RuntimeException();
    }

    public static String ConvertObjectToJson(Gson gson , Object obj)  {
        return gson.toJson(obj);
    }


    public static Object ConvertJsonToObject(Gson gson , String json, Class target)  {
        return gson.fromJson(json, target);
    }
}
