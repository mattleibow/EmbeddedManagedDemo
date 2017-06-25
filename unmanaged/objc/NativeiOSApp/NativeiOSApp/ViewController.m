//
//  ViewController.m
//  NativeiOSApp
//
//  Created by Matthew Leibowitz on 2017/06/24.
//  Copyright © 2017 Matthew Leibowitz. All rights reserved.
//

#import "ViewController.h"

#import <ManagediOSLibrary/ManagediOSLibrary.h>

@interface ViewController ()

@end

@implementation ViewController
{
    ManagediOSLibrary_CalculatorProxy *_calculator;
    __weak IBOutlet ManagediOSLibrary_CalculatorView *_calcView;
}

- (void)viewDidLoad {
    [super viewDidLoad];
    
    _calculator = (ManagediOSLibrary_CalculatorProxy *)_calcView.calculator;
    
    _calcView.buttonColor = [UIColor whiteColor];
    _calcView.layer.cornerRadius = 6;
    _calcView.buttonMargin = 12;
    _calcView.buttonPadding = 6;
    _calcView.buttonCornerRadius = 3;
}

- (IBAction)onReadStateTapped {
    NSDecimalNumber *prev = _calculator.previousOperand;
    ManagedLibrary_Operation op = _calculator.operation;
    NSDecimalNumber *operand = _calculator.operand;
    BOOL hasOperand = _calculator.hasOperand;
    
    NSString *state = [NSString stringWithFormat:
                       @"The current state of the calculator is:\nOperand 1: %@\nOperator: %@\nOperand 2: %@\nHas Operand 2: %@",
                       [self stringFromDecimal: prev],
                       [ManagedLibrary_Calculator getSymbolOp: op],
                       [self stringFromDecimal: operand],
                       hasOperand ? @"Yes" : @"No"];
    
    UIAlertView *alert = [[UIAlertView alloc] initWithTitle: @"Calculator State"
                                                    message: state
                                                   delegate: nil
                                          cancelButtonTitle: @"OK"
                                          otherButtonTitles: nil];
    [alert show];
}

- (NSString *)stringFromDecimal:(NSDecimalNumber*)number {
    NSNumberFormatter* formatter = [[NSNumberFormatter alloc] init];
    formatter.usesSignificantDigits = true;
    formatter.maximumSignificantDigits = 100;
    formatter.groupingSeparator = @"";
    formatter.numberStyle = NSNumberFormatterDecimalStyle;
    return [formatter stringFromNumber: number];
}

@end
