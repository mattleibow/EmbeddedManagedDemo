package com.example.matthew.myapplication;

import android.content.Context;
import android.support.test.InstrumentationRegistry;
import android.support.test.runner.AndroidJUnit4;

import org.junit.Test;
import org.junit.runner.RunWith;

import managedlibrary_dll.managedlibrary.Calculator;
import managedlibrary_dll.managedlibrary.Operation;

import static org.junit.Assert.*;

/**
 * Instrumentation test, which will execute on an Android device.
 *
 * @see <a href="http://d.android.com/tools/testing">Testing documentation</a>
 */
@RunWith(AndroidJUnit4.class)
public class CalculatorTest {
    @Test
    public void useAppContext() throws Exception {
        // Context of the app under test.
        Context appContext = InstrumentationRegistry.getTargetContext();

        assertEquals("com.example.matthew.myapplication", appContext.getPackageName());
    }

    @Test
    public void testSimpleAddition() throws Exception {
        Calculator calculator = new Calculator();

        // make sure that we are blank
        assertFalse(calculator.getHasOperand());
        assertEquals(calculator.getOperand(), 0.0, 0.0001);
        assertEquals(calculator.getPreviousOperand(), 0.0, 0.0001);
        assertEquals(calculator.getOperation(), Operation.None);

        // now start a simple add
        calculator.setOperand(12.3);
        // test
        assertTrue(calculator.getHasOperand());
        assertEquals(calculator.getOperand(), 12.3, 0.0001);
        assertEquals(calculator.getPreviousOperand(), 0.0, 0.0001);
        assertEquals(calculator.getOperation(), Operation.None);

        // do the add
        calculator.performOperation(Operation.Add);
        // test
        assertFalse(calculator.getHasOperand());
        assertEquals(calculator.getOperand(), 0.0, 0.0001);
        assertEquals(calculator.getPreviousOperand(), 12.3, 0.0001);
        assertEquals(calculator.getOperation(), Operation.Add);

        // the next number
        calculator.setOperand(32.1);
        // test
        assertTrue(calculator.getHasOperand());
        assertEquals(calculator.getOperand(), 32.1, 0.0001);
        assertEquals(calculator.getPreviousOperand(), 12.3, 0.0001);
        assertEquals(calculator.getOperation(), Operation.Add);


        // the equals
        calculator.performOperation(Operation.Equals);
        // test
        assertFalse(calculator.getHasOperand());
        assertEquals(calculator.getOperand(), 0.0, 0.0001);
        assertEquals(calculator.getPreviousOperand(), 44.4, 0.0001);
        assertEquals(calculator.getOperation(), Operation.None);
    }
}
