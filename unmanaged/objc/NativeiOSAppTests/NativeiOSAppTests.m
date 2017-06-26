//
//  NativeiOSAppTests.m
//  NativeiOSAppTests
//
//  Created by Matthew Leibowitz on 2017/06/24.
//  Copyright © 2017 Matthew Leibowitz. All rights reserved.
//

#import <XCTest/XCTest.h>

#import <ManagediOSLibrary/ManagediOSLibrary.h>

@interface NativeiOSAppTests : XCTestCase

@end

@implementation NativeiOSAppTests
{
    ManagedLibrary_Calculator *_calculator;
}

- (void)setUp {
    [super setUp];
    
    _calculator = [[ManagedLibrary_Calculator alloc] init];
}

- (void)tearDown {
    [_calculator clear];
    _calculator = nil;
    
    [super tearDown];
}

- (void)testSimpleAddition {
    // make sure that we are blank
    XCTAssertFalse(_calculator.hasOperand);
    XCTAssertEqual(_calculator.operand, [NSDecimalNumber zero]);
    XCTAssertEqual(_calculator.previousOperand, [NSDecimalNumber zero]);
    XCTAssertEqual(_calculator.operation, ManagedLibrary_OperationNone);
    
    // now start a simple add
    _calculator.operand = [NSDecimalNumber decimalNumberWithDecimal: [@12.3 decimalValue]];
    // test
    XCTAssertTrue(_calculator.hasOperand);
    XCTAssertEqual(_calculator.operand.floatValue, 12.3f);
    XCTAssertEqual(_calculator.previousOperand, [NSDecimalNumber zero]);
    XCTAssertEqual(_calculator.operation, ManagedLibrary_OperationNone);
    
    // do the add
    [_calculator performOperationOp:ManagedLibrary_OperationAdd];
    // test
    XCTAssertFalse(_calculator.hasOperand);
    XCTAssertEqual(_calculator.operand, [NSDecimalNumber zero]);
    XCTAssertEqual(_calculator.previousOperand.floatValue, 12.3f);
    XCTAssertEqual(_calculator.operation, ManagedLibrary_OperationAdd);
    
    // the next number
    _calculator.operand = [NSDecimalNumber decimalNumberWithDecimal: [@32.1 decimalValue]];
    // test
    XCTAssertFalse(_calculator.hasOperand);
    XCTAssertEqual(_calculator.operand.floatValue, 32.1f);
    XCTAssertEqual(_calculator.previousOperand.floatValue, 12.3f);
    XCTAssertEqual(_calculator.operation, ManagedLibrary_OperationAdd);
    
    // the equals
    [_calculator performOperationOp:ManagedLibrary_OperationEquals];
    // test
    XCTAssertFalse(_calculator.hasOperand);
    XCTAssertEqual(_calculator.operand, [NSDecimalNumber zero]);
    XCTAssertEqual(_calculator.previousOperand.floatValue, 44.4f);
    XCTAssertEqual(_calculator.operation, ManagedLibrary_OperationNone);
}

// - (void)testPerformanceExample {
//     // This is an example of a performance test case.
//     [self measureBlock:^{
//         // Put the code you want to measure the time of here.
//     }];
// }

@end
