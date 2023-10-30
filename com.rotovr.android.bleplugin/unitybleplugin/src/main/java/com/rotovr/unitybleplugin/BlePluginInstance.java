package com.rotovr.unitybleplugin;

import android.Manifest;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothGatt;
import android.bluetooth.BluetoothGattCharacteristic;
import android.bluetooth.BluetoothGattDescriptor;
import android.bluetooth.BluetoothGattService;
import android.bluetooth.le.BluetoothLeScanner;
import android.bluetooth.le.ScanCallback;
import android.bluetooth.le.ScanResult;
import android.content.Context;
import android.content.pm.PackageManager;
import android.os.Build;
import android.os.Handler;
import android.util.Base64;
import android.util.Log;

import androidx.annotation.RequiresApi;

import com.google.gson.Gson;
import com.rotovr.unitybleplugin.connection.ConnectionService;
import com.rotovr.unitybleplugin.model.DeviceDataModel;
import com.rotovr.unitybleplugin.model.MessageModel;
import com.rotovr.unitybleplugin.model.RotateToAngleModel;
import com.rotovr.unitybleplugin.utility.PluginUtility;
import com.unity3d.player.UnityPlayer;

import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.UUID;

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
    private static Gson gson;
    private DeviceDataModel m_CurrentDeviceModel;
    private byte[] mGattMessage;


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

        mGattMessage = new byte[19];

        ResetMessage();
    }

    void ResetMessage() {
        for (int i = 0; i < mGattMessage.length; i++) {
            mGattMessage[i] = (byte) (0 & 0xFF);
        }
    }

    @RequiresApi(api = Build.VERSION_CODES.M)
    @SuppressLint("MissingPermission")
    public static BlePluginInstance GetInstance() {
        if (m_Instance == null)
            m_Instance = new BlePluginInstance();

        gson = new Gson();
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

            m_LeDeviceListAdapter.Clear();

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

                        DeviceDataModel model = new DeviceDataModel(device.getName(), device.getAddress());

                        SendToUnity(new MessageModel(MessageType.DeviceFound, model.toJson()));
                    }
                }
            };

    @SuppressLint("MissingPermission")
    public void Connect(String data) {

        DeviceDataModel model = (DeviceDataModel) PluginUtility.ConvertJsonToObject(gson, data, DeviceDataModel.class);
        m_CurrentDeviceModel = model;

        BluetoothDevice device = m_LeDeviceListAdapter.getItem(model.Address);

        if (device != null && !m_ConnectedServers.containsKey(device)) {

            ConnectionService service = new ConnectionService(this);
            device.connectGatt(UnityPlayer.currentActivity.getApplicationContext(), true, service.gattCallback);

            m_ConnectedServers.put(device, service);

        } else {
            UnityLogError("BluetoothDevice hasn't been discovered yet");

        }
    }

    @SuppressLint("MissingPermission")
    public void DeviceConnected() {

        SendToUnity(new MessageModel(MessageType.Connected, ""));
    }

    public void ConnectedToGattServer(BluetoothGatt gattServer) {
        if (!m_LeGattServers.containsKey(gattServer.getDevice())) {
            m_LeGattServers.put(gattServer.getDevice(), gattServer);
        }

        SendToUnity(new MessageModel(MessageType.ConnectedToGattServer));
    }

    @SuppressLint("MissingPermission")
    public void Disconnect(String data) {
        DeviceDataModel model = (DeviceDataModel) PluginUtility.ConvertJsonToObject(gson, data, DeviceDataModel.class);

        BluetoothDevice device = m_LeDeviceListAdapter.getItem(model.Address);
        BluetoothGatt gatt = m_LeGattServers.get(device);

        if (gatt != null) {
            gatt.close();
            gatt.disconnect();

            m_ConnectedServers.remove(m_LeDeviceListAdapter.getItem(model.Address));
            m_LeGattServers.remove(device);
        } else {
            UnityLogError("Can't find connected device with address " + model.Address);
        }

        SendToUnity(new MessageModel(MessageType.Disconnected, model.toJson()));
    }

    @SuppressLint("MissingPermission")
    public void DeviceDisconnected(String deviceAddress) {
        BluetoothDevice device = m_LeDeviceListAdapter.getItem(deviceAddress);
        BluetoothGatt gatt = m_LeGattServers.get(device);

        BluetoothGatt gattServer = GetServer(m_CurrentDeviceModel.Address);
        BluetoothGattService gattService = GetService(gattServer, "ffc0");
        BluetoothGattCharacteristic characteristic = GetCharacteristic(gattService, "ffc9");

        gattServer.setCharacteristicNotification(characteristic, false);

        UUID uuid = UUID.fromString("00002902-0000-1000-8000-00805f9b34fb");
        BluetoothGattDescriptor descriptor = characteristic.getDescriptor(uuid);
        descriptor.setValue(BluetoothGattDescriptor.DISABLE_NOTIFICATION_VALUE);
        gattServer.writeDescriptor(descriptor);

        if (gatt != null) {
            gatt.close();
            gatt.disconnect();

            m_ConnectedServers.remove(m_LeDeviceListAdapter.getItem(deviceAddress));
            m_LeGattServers.remove(device);
            m_LeDeviceListAdapter.RemoveDevice(device);
        } else {
            UnityLogError("Can't find connected device with address " + deviceAddress);
        }

        SendToUnity(new MessageModel(MessageType.Disconnected));
    }


    @SuppressLint("MissingPermission")
    public void DiscoveredService(BluetoothGatt gatt) {
        BluetoothGatt gattServer = GetServer(m_CurrentDeviceModel.Address);
        BluetoothGattService gattService = GetService(gattServer, "ffc0");
        BluetoothGattCharacteristic characteristic = GetCharacteristic(gattService, "ffc9");

        gattServer.setCharacteristicNotification(characteristic, true);

        UUID uuid = UUID.fromString("00002902-0000-1000-8000-00805f9b34fb");
        BluetoothGattDescriptor descriptor = characteristic.getDescriptor(uuid);
        descriptor.setValue(BluetoothGattDescriptor.ENABLE_NOTIFICATION_VALUE);
        gattServer.writeDescriptor(descriptor);

        SendToUnity(new MessageModel(MessageType.DeviceConnected));
    }

    public void CharacteristicValueChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic) {
        byte[] data = characteristic.getValue();

        UnityLogError("CharacteristicValueChanged. Angle: " + (data[5] & 0xFF) + "  " + (data[6] & 0xFF));
    }

    public void SetMode(String data) {
        ResetMessage();

        mGattMessage[0] = (byte) (0xF1 & 0xFF);
        mGattMessage[1] = (byte) 'S';
        mGattMessage[2] = (byte) (0x03 & 0xFF);
        mGattMessage[9] = (byte) 70;
        mGattMessage[11] = (byte) 40;
        mGattMessage[12] = (byte) 100;
        mGattMessage[14] = (byte) 1;
        byte sum = ByteSum(mGattMessage);
        mGattMessage[18] = sum;

        WriteToGattCharacteristic(m_CurrentDeviceModel.Address, "ffc0", "ffc9", mGattMessage);
    }

    public void Calibration() {

        ResetMessage();
        mGattMessage[0] = (byte) (0xF1 & 0xFF);
        mGattMessage[1] = (byte) (0x52 & 0xFF);
        mGattMessage[2] = (byte) 0x00;
        mGattMessage[3] = (byte) (1 & 0xFF);
        mGattMessage[4] = (byte) (100 & 0xFF);
        byte sum = ByteSum(mGattMessage);
        mGattMessage[18] = sum;


        WriteToGattCharacteristic(m_CurrentDeviceModel.Address, "ffc0", "ffc9", mGattMessage);
    }

    public void TurnOnAngle(String data) {

        ResetMessage();
        RotateToAngleModel model = (RotateToAngleModel) PluginUtility.ConvertJsonToObject(gson, data, RotateToAngleModel.class);

        mGattMessage[0] = (byte) (0xF1 & 0xFF);

        if (model.Direction.equals("Right")) {
            mGattMessage[1] = (byte) (0x52 & 0xFF);
        } else {
            mGattMessage[1] = (byte) (0x4C & 0xFF);
        }

        if (model.Angle == 360)
            model.Angle -= 1;

        if (model.Angle >= 256) {
            mGattMessage[2] = (byte) 0x01;
            mGattMessage[3] = (byte) ((model.Angle - 256) & 0xFF);
        } else {
            mGattMessage[2] = (byte) 0x00;
            mGattMessage[3] = (byte) (model.Angle & 0xFF);

        }
        mGattMessage[4] = (byte) (model.Power & 0xFF);

        byte sum = ByteSum(mGattMessage);
        mGattMessage[18] = sum;

        WriteToGattCharacteristic(m_CurrentDeviceModel.Address, "ffc0", "ffc9", mGattMessage);
        UnityLogError("Try to turn " + model.Direction + " on angle " + model.Angle + "   with power " + model.Power);
    }

    public void TurnToAngle(String data) {

    }

    private byte ByteSum(byte[] blk) {
        byte sum = 0;

        for (int i = 0; i <= 17; i++) {
            sum = (byte) ((sum + blk[i]) & 0xff);

        }
        return sum;
    }

    @SuppressLint("MissingPermission")
    public void ReadFromCharacteristic(String device, String service, String characteristic) {

        BluetoothGatt gattServer = GetServer(device);
        BluetoothGattService gattService = GetService(gattServer, service);
        BluetoothGattCharacteristic gattCharacteristic = GetCharacteristic(gattService, characteristic);

        if ((gattCharacteristic.getProperties() & BluetoothGattCharacteristic.PROPERTY_READ) != 0) {
            UnityLogError(" ReadFromCharacteristic CAN READ");
        } else {
            UnityLogError(" ReadFromCharacteristic CAN NOT READ");
            return;
        }
        boolean read = gattServer.readCharacteristic(gattCharacteristic);

        UnityLogError(" ReadFromCharacteristic success: " + read);
    }

    @SuppressLint("MissingPermission")
    public void WriteToGattCharacteristic(String device, String service, String characteristic, byte[] message) {
        BluetoothGatt gattServer = GetServer(device);
        BluetoothGattService gattService = GetService(gattServer, service);
        BluetoothGattCharacteristic gattCharacteristic = GetCharacteristic(gattService, characteristic);

        gattCharacteristic.setValue(message);
        boolean success = gattServer.writeCharacteristic(gattCharacteristic);

        UnityLogError("WriteToGattCharacteristic success: " + success);
    }

    BluetoothGatt GetServer(String device) {
        BluetoothDevice bDevice = m_LeDeviceListAdapter.getItem(device);
        return m_LeGattServers.get(bDevice);
    }

    BluetoothGattService GetService(BluetoothGatt gattServer, String service) {
        UUID serviceUUID = UUID.fromString("0000" + service + "-0000-1000-8000-00805f9b34fb");
        return gattServer.getService(serviceUUID);
    }

    BluetoothGattCharacteristic GetCharacteristic(BluetoothGattService gattService, String characteristic) {
        UUID gattUUID = UUID.fromString("0000" + characteristic + "-0000-1000-8000-00805f9b34fb");
        return gattService.getCharacteristic(gattUUID);
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
