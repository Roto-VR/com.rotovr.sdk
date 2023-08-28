package com.rotovr.unitybleplugin;

import android.Manifest;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothGatt;
import android.bluetooth.le.BluetoothLeScanner;
import android.bluetooth.le.ScanCallback;
import android.bluetooth.le.ScanResult;
import android.content.Context;
import android.content.pm.PackageManager;
import android.os.Build;
import android.os.Handler;
import android.util.Log;

import androidx.annotation.RequiresApi;

import com.rotovr.unitybleplugin.connection.ConnectionService;
import com.rotovr.unitybleplugin.model.BleObject;
import com.rotovr.unitybleplugin.model.DeviceModel;
import com.rotovr.unitybleplugin.model.MessageModel;
import com.unity3d.player.UnityPlayer;

import java.util.HashMap;
import java.util.Map;

public class BlePluginInstance {

    private static BlePluginInstance m_Instance = null;
    private static final String LOGCAT = "AndroidBlePlugin";
    private static final String m_UnityBLEReceiver = "BleAdapter";
    private static final String m_UnityBLEMethod = "OnBleStringMessage";
    private static final String m_UnityLogCommand = "OnLogMessage";
    private static final String m_UnityLogErrorCommand = "OnLogErrorMessage";
    private static BluetoothAdapter m_BluetoothAdapter = null;
    public static BluetoothLeScanner m_BluetoothLeScanner = null;

    public static LeDeviceListAdapter m_LeDeviceListAdapter = null;
    private static Map<BluetoothDevice, BluetoothGatt> m_LeGattServers = null;

    private static Map<BluetoothDevice, ConnectionService> m_ConnectedServers = null;
    private static Context m_Context;
    private static byte[] m_MessageData;
    public static boolean m_Scanning = false;
    private Handler m_Handler = new Handler();

    @RequiresApi(api = Build.VERSION_CODES.M)
    @SuppressLint("MissingPermission")
    public BlePluginInstance() {
        CheckPermissions(UnityPlayer.currentActivity.getApplicationContext(), UnityPlayer.currentActivity);

        m_BluetoothAdapter = BluetoothAdapter.getDefaultAdapter();
        m_BluetoothAdapter.enable();

        m_BluetoothLeScanner = m_BluetoothAdapter.getBluetoothLeScanner();

        m_LeDeviceListAdapter = new LeDeviceListAdapter();

        m_LeGattServers = new HashMap<BluetoothDevice, BluetoothGatt>();
        m_ConnectedServers = new HashMap<BluetoothDevice, ConnectionService>();
    }

    @RequiresApi(api = Build.VERSION_CODES.M)
    @SuppressLint("MissingPermission")
    public static BlePluginInstance GetInstance() {
        if (m_Instance == null)
            m_Instance = new BlePluginInstance();


        m_BluetoothAdapter = BluetoothAdapter.getDefaultAdapter();
        m_BluetoothAdapter.enable();

        m_BluetoothLeScanner = m_BluetoothAdapter.getBluetoothLeScanner();

        m_LeDeviceListAdapter = new LeDeviceListAdapter();

        m_LeGattServers = new HashMap<BluetoothDevice, BluetoothGatt>();
        m_ConnectedServers = new HashMap<BluetoothDevice, ConnectionService>();


        m_Context = UnityPlayer.currentActivity.getApplicationContext();

        //Checks to see if the device features Bluetooth Low Energy
        if (!m_Context.getPackageManager().hasSystemFeature(PackageManager.FEATURE_BLUETOOTH_LE)) {
            UnityLogError("Device doesn't support Bluetooth Low Energy");
        }
        m_MessageData = new byte[20];

        if (!m_BluetoothAdapter.isEnabled())
            m_BluetoothAdapter.enable();

        SendToUnity(new MessageModel(MessageType.Initialized));

        return m_Instance;
    }

    @RequiresApi(api = Build.VERSION_CODES.M)
    static void CheckPermissions(Context context, Activity activity) {
        if (context.checkSelfPermission(Manifest.permission.BLUETOOTH) != PackageManager.PERMISSION_GRANTED
                || context.checkSelfPermission(Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED
                || context.checkSelfPermission(Manifest.permission.BLUETOOTH_SCAN) != PackageManager.PERMISSION_GRANTED
                || context.checkSelfPermission(Manifest.permission.BLUETOOTH_ADMIN) != PackageManager.PERMISSION_GRANTED
                || context.checkSelfPermission(Manifest.permission.BLUETOOTH_CONNECT) != PackageManager.PERMISSION_GRANTED) {
            activity.requestPermissions(new String[]{Manifest.permission.BLUETOOTH, Manifest.permission.BLUETOOTH_ADMIN, Manifest.permission.ACCESS_FINE_LOCATION, Manifest.permission.BLUETOOTH_SCAN, Manifest.permission.BLUETOOTH_CONNECT}, 1);
        }
    }

    @SuppressLint("MissingPermission")
    public void Scan() {

        if (!m_Scanning) {
            this.m_Handler.postDelayed(new Runnable() {
                @SuppressLint("MissingPermission")
                @Override
                public void run() {
                    m_Scanning = false;
                    m_BluetoothLeScanner.stopScan(BleScanCallback);

                    SendToUnity(new MessageModel(MessageType.FinishedDiscovering));
                }
            }, 10000);

            m_Scanning = true;
            m_BluetoothLeScanner.startScan(BleScanCallback);

            UnityLog("Starting Scan");

            return;
        } else {
            UnityLog("BLE Manager is already scanning.");
        }
    }

    private ScanCallback BleScanCallback =
            new ScanCallback() {
                @SuppressLint("MissingPermission")
                @Override
                public void onScanResult(int callbackType, ScanResult result) {
                    super.onScanResult(callbackType, result);
                    BluetoothDevice device = result.getDevice();
                    if (m_LeDeviceListAdapter.AddDevice(device)) {

                        DeviceModel model = new DeviceModel(device.getName(), device.getAddress());

                        SendToUnity(new MessageModel(MessageType.DeviceFound, model.toJson()));
                    }
                }
            };

    public void Connect() {

    }


    public void Disconnect() {

    }

    public void TurnOnAngle() {

    }

    public void TurnToAngle() {

    }


    public static void SendToUnity(MessageModel messageModel) {
        UnityPlayer.UnitySendMessage(m_UnityBLEReceiver, m_UnityBLEMethod, messageModel.toJson());
    }

    public static void UnityLog(String message) {
        UnityPlayer.UnitySendMessage(m_UnityBLEReceiver, m_UnityLogCommand, message);
    }

    public static void UnityLogError(String message) {
        UnityPlayer.UnitySendMessage(m_UnityBLEReceiver, m_UnityLogErrorCommand, message);
    }

    public static void AndroidLog(String message) {
        Log.i("LOGCAT", message);
    }
}
