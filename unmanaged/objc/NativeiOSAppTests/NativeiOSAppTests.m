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
    XCTAssertEqualWithAccuracy(_calculator.operand, 0.0, 0.001);
    XCTAssertEqualWithAccuracy(_calculator.previousOperand, 0.0, 0.001);
    XCTAssertEqual(_calculator.operation, ManagedLibrary_OperationNone);
    
    // now start a simple add
    _calculator.operand = 12.3;
    // test
    XCTAssertTrue(_calculator.hasOperand);
    XCTAssertEqualWithAccuracy(_calculator.operand, 12.3, 0.001);
    XCTAssertEqualWithAccuracy(_calculator.previousOperand, 0.0, 0.001);
    XCTAssertEqual(_calculator.operation, ManagedLibrary_OperationNone);
    
    // do the add
    [_calculator performOperationOp:ManagedLibrary_OperationAdd];
    // test
    XCTAssertFalse(_calculator.hasOperand);
    XCTAssertEqualWithAccuracy(_calculator.operand, 0.0, 0.001);
    XCTAssertEqualWithAccuracy(_calculator.previousOperand, 12.3, 0.001);
    XCTAssertEqual(_calculator.operation, ManagedLibrary_OperationAdd);
    
    // the next number
    _calculator.operand = 32.1;
    // test
    XCTAssertFalse(_calculator.hasOperand);
    XCTAssertEqualWithAccuracy(_calculator.operand, 32.1, 0.001);
    XCTAssertEqualWithAccuracy(_calculator.previousOperand, 12.3, 0.001);
    XCTAssertEqual(_calculator.operation, ManagedLibrary_OperationAdd);
    
    // the equals
    [_calculator performOperationOp:ManagedLibrary_OperationEquals];
    // test
    XCTAssertFalse(_calculator.hasOperand);
    XCTAssertEqualWithAccuracy(_calculator.operand, 0.0, 0.001);
    XCTAssertEqualWithAccuracy(_calculator.previousOperand, 44.4, 0.001);
    XCTAssertEqual(_calculator.operation, ManagedLibrary_OperationNone);
}

// - (void)testPerformanceExample {
//     // This is an example of a performance test case.
//     [self measureBlock:^{
//         // Put the code you want to measure the time of here.
//     }];
// }

@end
