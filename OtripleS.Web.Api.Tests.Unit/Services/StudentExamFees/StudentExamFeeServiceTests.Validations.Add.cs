﻿//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.StudentExamFees;
using OtripleS.Web.Api.Models.StudentExamFees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExamFees
{
    public partial class StudentExamFeeServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenStudentExamFeeIsNullAndLogItAsync()
        {
            // given
            StudentExamFee randomStudentExamFee = null;
            StudentExamFee nullStudentExamFee = randomStudentExamFee;
            var nullStudentExamFeeException = new NullStudentExamFeeException();

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(nullStudentExamFeeException);

            // when
            ValueTask<StudentExamFee> createStudentExamFeeTask =
                this.studentExamFeeService.AddStudentExamFeeAsync(nullStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                createStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
