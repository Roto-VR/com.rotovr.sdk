package com.rotovr.unitybleplugin.connection;
import android.annotation.SuppressLint;
import android.bluetooth.BluetoothGatt;
import android.bluetooth.BluetoothGattCallback;
import android.bluetooth.BluetoothGattCharacteristic;
import android.bluetooth.BluetoothGattDescriptor;

import android.bluetooth.BluetoothProfile;
import android.content.Intent;
import android.os.IBinder;

import androidx.annotation.Nullable;

import com.rotovr.unitybleplugin.BlePluginInstance;

public class ConnectionService {
    public int connectionState = 0;
    private final BlePluginInstance m_BlePluginInstance;

    public ConnectionService(BlePluginInstance bleManager) {
        m_BlePluginInstance = bleManager;
    }

    @Nullable
    public IBinder onBind(Intent intent) {
        return null;
    }

    public final BluetoothGattCallback gattCallback = new BluetoothGattCallback() {
        @SuppressLint("MissingPermission")
        @Override
        public void onConnectionStateChange(BluetoothGatt gatt, int status, int newState) {
            if (newState == BluetoothProfile.STATE_CONNECTED) {
                BlePluginInstance.UnityLog("onConnectionStateChange to state " + "CONNECTED");

                connectionState = newState;
                m_BlePluginInstance.ConnectedToGattServer(gatt);

                gatt.discoverServices();

                m_BlePluginInstance.DeviceConnected();

            } else if (newState == BluetoothProfile.STATE_DISCONNECTED) {

                connectionState = newState;
                m_BlePluginInstance.DisconnectDevice(gatt.getDevice().getAddress());

                BlePluginInstance.UnityLog("onConnectionStateChange to state " + "DISCONNECTED");
            }

        }

        @Override
        public void onServicesDiscovered(BluetoothGatt gatt, int status) {
            if (status == BluetoothGatt.GATT_SUCCESS) {
                m_BlePluginInstance.DiscoveredService(gatt);
            }
        }

        @Override
        public void onCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic) {
            m_BlePluginInstance.CharacteristicValueChanged(gatt, characteristic);
        }

        @Override
        public void onCharacteristicRead(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, int status) {
            byte[] data = characteristic.getValue();

            BlePluginInstance.UnityLog("On Characteristic Read");
        }

        @Override
        public void onDescriptorWrite(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, int status) {
            super.onDescriptorWrite(gatt, descriptor, status);
        }
    };
}
