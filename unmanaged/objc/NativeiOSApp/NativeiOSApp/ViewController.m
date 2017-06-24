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

- (IBAction)OnReadStateTapped:(id)sender {    
    NSDecimalNumber *prev = _calculator.previousOperand;
    ManagedLibrary_Operation op = _calculator.operation;
    NSDecimalNumber *operand = _calculator.operand;
    BOOL hasOperand = _calculator.hasOperand;
}

@end
