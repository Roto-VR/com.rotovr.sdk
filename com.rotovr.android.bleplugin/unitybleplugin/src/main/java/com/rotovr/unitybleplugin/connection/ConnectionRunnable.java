package com.rotovr.unitybleplugin.connection;

import android.bluetooth.BluetoothDevice;
import android.content.Context;

import com.rotovr.unitybleplugin.BlePluginInstance;

public class ConnectionRunnable implements Runnable {
    private final BlePluginInstance m_BlePlugin;
    private final BluetoothDevice m_Device;

    private final Context m_ApplicationContext;

    private final ConnectionService m_ConnectionService;

    public ConnectionRunnable(BlePluginInstance bleManager, BluetoothDevice device, Context applicationContext) {
        m_BlePlugin = bleManager;
        m_Device = device;

        m_ApplicationContext = applicationContext;

        m_ConnectionService = new ConnectionService(m_BlePlugin);
    }

    @Override
    public void run() {

    }
}
