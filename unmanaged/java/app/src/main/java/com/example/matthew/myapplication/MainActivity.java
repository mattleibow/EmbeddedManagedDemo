package com.example.matthew.myapplication;

import android.os.Debug;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import managedlibrary_dll.managedlibrary.Calculator;
import managedlibrary_dll.managedlibrary.Operation;

public class MainActivity extends AppCompatActivity {

    private Calculator _calculator;
    private TextView _display;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        _display = (TextView)findViewById(R.id.display);

        _calculator = new Calculator();
        String t = _calculator.getEX();
    }

    public void onButtonClick(View v) {
        Button btn = (v instanceof Button) ? (Button) v : null;
        if (btn != null) {
            String symbol = btn.getText().toString().trim();

            _display.setText(_calculator.handleSymbol(_display.getText().toString(), symbol));
        }
    }

    public void onReadStateClick(View v) {
        double prev = _calculator.getPreviousOperand();
        Operation op = _calculator.getOperation();
        double operand = _calculator.getOperand();
        boolean hasOperand = _calculator.getHasOperand();

        String state = String.format(
                "The current state of the calculator is:\nOperand 1: %1$f\nOperator: %2$s\nOperand 2: %3$f\nHas Operand 2: %4$s",
                prev,
                Calculator.getSymbol(op),
                operand,
                hasOperand ? "Yes" : "No");

        new AlertDialog.Builder(this)
                .setTitle("Calculator State")
                .setMessage(state)
                .setPositiveButton("OK", null)
                .create()
                .show();
    }
}
