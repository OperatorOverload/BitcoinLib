using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using BitcoinLib.Responses;
using BitcoinLib.ExceptionHandling;


namespace BitcoinLib.Services
{
    public sealed partial class BitcoinService : IBitcoinService
    {
        public BitcoinLib.Requests.CreateRawTransaction.CreateRawTransactionRequest BuildRawTransactionRequest(List<ListUnspentResponse> unspent, Dictionary<string, decimal> outputAddressesAndAmounts, decimal maximumAllowedFee = 0.0m)
        {
            List<BitcoinLib.Requests.CreateRawTransaction.CreateRawTransactionInput> txInputs = new List<BitcoinLib.Requests.CreateRawTransaction.CreateRawTransactionInput>();

            decimal totalAmountInputs = 0m;

            foreach (var input in unspent)
            {
                var newInput = new BitcoinLib.Requests.CreateRawTransaction.CreateRawTransactionInput();

                newInput.TransactionId = input.TxId;
                newInput.Output = input.VOut;
                totalAmountInputs += input.Amount;

                txInputs.Add(newInput);
            }

            decimal totalAmountOutputs = outputAddressesAndAmounts.Sum(t => t.Value);

            decimal actualFee = totalAmountInputs - totalAmountOutputs;

            if (actualFee < 0)
                throw new RawTransactionAmountInvalidException(String.Format("Amount of input ({0}) is less than outputs ({1})", totalAmountInputs, totalAmountOutputs));
            else if (actualFee > maximumAllowedFee)
                throw new RawTransactionExcessiveFeeException(actualFee, maximumAllowedFee);
                

            var rawReq = new BitcoinLib.Requests.CreateRawTransaction.CreateRawTransactionRequest(txInputs, outputAddressesAndAmounts);

            return rawReq;
        }

        public string BuildAndSendRawTransaction(List<ListUnspentResponse> unspent, Dictionary<string, decimal> outputAddressesAndAmounts, decimal maximumAllowedFee = 0.0m)
        {
            var rawTxRequest = BuildRawTransactionRequest(unspent, outputAddressesAndAmounts, maximumAllowedFee);

            return SignAndSendRawTransactionRequest(rawTxRequest);
        }

        private string SignAndSendRawTransactionRequest(BitcoinLib.Requests.CreateRawTransaction.CreateRawTransactionRequest rawTxRequest)
        {
            var rawTx = CreateRawTransaction(rawTxRequest);

            BitcoinLib.Requests.SignRawTransaction.SignRawTransactionRequest rawTxSignRequest = new BitcoinLib.Requests.SignRawTransaction.SignRawTransactionRequest(rawTx);

            var signedTx = SignRawTransaction(rawTxSignRequest);

            string transactionId = SendRawTransaction(signedTx.Hex);

            return transactionId;
        }
    }
}
