﻿using System;
using System.Collections.Generic;
using System.IO;
using BrightstarDB.Storage;

namespace BrightstarDB.Client
{
    ///<summary>
    /// The main interface for interacting with the Brightstar database.
    ///</summary>
    public interface IBrightstarService
    {
        /// <summary>
        /// Returns the timestamp provided by the server on its last response.
        /// </summary>
        /// <remarks>This property will be null if no operation has been invoked, or 
        /// if the client is an embedded client.</remarks>
        DateTime? LastResponseTimestamp { get; }

        /// <summary>
        /// List the names of the stores managed by this Brightstar server
        /// </summary>
        /// <returns>An enumeration over the names of the stores managed by the Brightstar server</returns>
        IEnumerable<string> ListStores();

        /// <summary>
        /// Create a new store
        /// </summary>
        /// <param name="storeName">The name of the store to be created</param>
        void CreateStore(string storeName);

        /// <summary>
        /// Create a new store with a specific persistence type for the main store indexes
        /// </summary>
        /// <param name="storeName">The name of the store to be created</param>
        /// <param name="persistenceType">The type of persistence to use for the main store indexes</param>
        void CreateStore(string storeName, PersistenceType persistenceType);

        /// <summary>
        /// Delete the named store
        /// </summary>
        /// <param name="storeName">The name of the store to be deleted</param>
        void DeleteStore(string storeName);

        /// <summary>
        /// Checks to see if the named store already exists
        /// </summary>
        /// <param name="storeName">The name of the store to test for</param>
        /// <returns>True if store exists, false otherwise</returns>
        bool DoesStoreExist(string storeName);

        /// <summary>
        /// Query the store using a SPARQL query
        /// </summary>
        /// <param name="storeName">The name of the store to query</param>
        /// <param name="queryExpression">SPARQL query string</param>
        /// <param name="ifNotModifiedSince">OPTIONAL : If this parameter is provided and the store has not been changed since the time specified,
        /// a BrightstarClientException will be raised with the message "Store not modified".</param>
        /// <param name="resultsFormat">OPTIONAL: Specifies the serialization format for the SPARQL results. Defaults to <see cref="SparqlResultsFormat.Xml"/></param>
        /// <returns>A stream containing XML SPARQL result XML</returns>
        Stream ExecuteQuery(string storeName, string queryExpression, DateTime? ifNotModifiedSince = null, SparqlResultsFormat resultsFormat = null);

        /// <summary>
        /// Query the store using a SPARQL query
        /// </summary>
        /// <param name="storeName">The name of the store to query</param>
        /// <param name="queryExpression">SPARQL query string</param>
        /// <param name="defaultGraphUri">The URI of the graph that will be the default graph for the query</param>
        /// <param name="ifNotModifiedSince">OPTIONAL : If this parameter is provided and the store has not been changed since the time specified,
        /// a BrightstarClientException will be raised with the message "Store not modified".</param>
        /// <param name="resultsFormat">OPTIONAL: Specifies the serialization format for the SPARQL results. Defaults to <see cref="SparqlResultsFormat.Xml"/></param>
        /// <returns>A stream containing XML SPARQL result XML</returns>
        Stream ExecuteQuery(string storeName, string queryExpression, string defaultGraphUri, DateTime? ifNotModifiedSince = null, SparqlResultsFormat resultsFormat = null);
        
        /// <summary>
        /// Query the store using a SPARQL query
        /// </summary>
        /// <param name="storeName">The name of the store to query</param>
        /// <param name="queryExpression">SPARQL query string</param>
        /// <param name="defaultGraphUris">An enumeration over the URIs of the graphs that will be taken together as the default graph for the query</param>
        /// <param name="ifNotModifiedSince">OPTIONAL : If this parameter is provided and the store has not been changed since the time specified,
        /// a BrightstarClientException will be raised with the message "Store not modified".</param>
        /// <param name="resultsFormat">OPTIONAL: Specifies the serialization format for the SPARQL results. Defaults to <see cref="SparqlResultsFormat.Xml"/></param>
        /// <returns>A stream containing XML SPARQL result XML</returns>
        Stream ExecuteQuery(string storeName, string queryExpression, IEnumerable<string> defaultGraphUris, DateTime? ifNotModifiedSince = null, SparqlResultsFormat resultsFormat = null);


        /// <summary>
        /// Query a specific commit point of a store
        /// </summary>
        /// <param name="commitPoint">The commit point be queried</param>
        /// <param name="queryExpression">The SPARQL query string</param>
        /// <param name="resultsFormat">OPTIONAL: Specifies the serialization format for the SPARQL results. Defaults to <see cref="SparqlResultsFormat.Xml"/></param>
        /// <returns>A stream containing XML SPARQL results</returns>
        Stream ExecuteQuery(ICommitPointInfo commitPoint, string queryExpression, SparqlResultsFormat resultsFormat = null);

        /// <summary>
        /// Query a specific commit point of a store
        /// </summary>
        /// <param name="commitPoint">The commit point be queried</param>
        /// <param name="queryExpression">The SPARQL query string</param>
        /// <param name="defaultGraphUri">The URI of the default graph for the query</param>
        /// <param name="resultsFormat">OPTIONAL: Specifies the serialization format for the SPARQL results. Defaults to <see cref="SparqlResultsFormat.Xml"/></param>
        /// <returns>A stream containing XML SPARQL results</returns>
        Stream ExecuteQuery(ICommitPointInfo commitPoint, string queryExpression, string defaultGraphUri, SparqlResultsFormat resultsFormat = null);

        /// <summary>
        /// Query a specific commit point of a store
        /// </summary>
        /// <param name="commitPoint">The commit point be queried</param>
        /// <param name="queryExpression">The SPARQL query string</param>
        /// <param name="defaultGraphUris">OPTIONAL: An enumeration over the URIs of the graphs that will be taken together as the default graph for the query. May be NULL to use the built-in default graph</param>
        /// <param name="resultsFormat">OPTIONAL: Specifies the serialization format for the SPARQL results. Defaults to <see cref="SparqlResultsFormat.Xml"/></param>
        /// <returns>A stream containing XML SPARQL results</returns>
        Stream ExecuteQuery(ICommitPointInfo commitPoint, string queryExpression, IEnumerable<string> defaultGraphUris, SparqlResultsFormat resultsFormat = null);

#if PORTABLE
        /// <summary>
        /// Execute an update transaction.
        /// </summary>
        /// <param name="storeName">The name of the store to modify</param>
        /// <param name="preconditions">NTriples or NQuads that must be in the store in order for the transaction to execute</param>
        /// <param name="deletePatterns">The delete patterns that will be removed from the store</param>
        /// <param name="insertData">The NTriples or NQuads data that will be inserted into the store.</param>
        /// <param name="defaultGraphUri">The URI of the default graph that the transaction will be applied to</param>
        /// <returns>A <see cref="JobInfo"/> instance for monitoring the status of the job</returns>
        /// <remarks>If <paramref name="preconditions"/>, <paramref name="deletePatterns"/> or <paramref name="insertData"/> contain
        /// quads, the graph URI specified by the quad will override the value provided by <paramref name="defaultGraphUri"/>. </remarks>
        IJobInfo ExecuteTransaction(string storeName, string preconditions, string deletePatterns, string insertData, string defaultGraphUri = Constants.DefaultGraphUri);

        /// <summary>
        /// Execute a SPARQL Update expression against a store
        /// </summary>
        /// <param name="storeName">The name of the store to be updated</param>
        /// <param name="updateExpression">The SPARQL Update expression to be applied</param>
        /// <returns>A <see cref="JobInfo"/> instance for monitoring the status of the job</returns>
        IJobInfo ExecuteUpdate(string storeName, string updateExpression);
#else
        /// <summary>
        /// Execute an update transaction.
        /// </summary>
        /// <param name="storeName">The name of the store to modify</param>
        /// <param name="preconditions">NTriples or NQuads that must be in the store in order for the transaction to execute</param>
        /// <param name="deletePatterns">The delete patterns that will be removed from the store</param>
        /// <param name="insertData">The NTriples or NQuads data that will be inserted into the store.</param>
        /// <param name="defaultGraphUri">The URI of the default graph that the transaction will be applied to</param>
        /// <param name="waitForCompletion">If set to true the method will block until the transaction completes</param>
        /// <returns>A <see cref="JobInfo"/> instance for monitoring the status of the job</returns>
        /// <remarks>If <paramref name="preconditions"/>, <paramref name="deletePatterns"/> or <paramref name="insertData"/> contain
        /// quads, the graph URI specified by the quad will override the value provided by <paramref name="defaultGraphUri"/>. </remarks>
        IJobInfo ExecuteTransaction(string storeName, string preconditions, string deletePatterns, string insertData, string defaultGraphUri = Constants.DefaultGraphUri, bool waitForCompletion = true);

        /// <summary>
        /// Execute a SPARQL Update expression against a store
        /// </summary>
        /// <param name="storeName">The name of the store to be updated</param>
        /// <param name="updateExpression">The SPARQL Update expression to be applied</param>
        /// <param name="waitForCompletion">If set to true, the method will block until the transaction completes</param>
        /// <returns>A <see cref="JobInfo"/> instance for monitoring the status of the job</returns>
        IJobInfo ExecuteUpdate(string storeName, string updateExpression, bool waitForCompletion = true);
#endif

        /// <summary>
        /// Gets the information about a job. Including status and any messages.
        /// </summary>
        /// <param name="storeName">Name of the store where the job is running</param>
        /// <param name="jobId">The Id of the job</param>
        /// <returns>A <see cref="JobInfo"/> instance for monitoring the status of the job</returns>
        IJobInfo GetJobInfo(string storeName, string jobId);

        /// <summary>
        /// Starts an import job.
        /// </summary>
        /// <param name="store">The store to perform the import to</param>
        /// <param name="fileName">The name of the file in brighhtstar\import folder to import.</param>
        /// <param name="graphUri">The URI of the default graph to import the data into. Defaults to <see cref="Constants.DefaultGraphUri"/></param>
        /// <returns>A <see cref="JobInfo"/> instance for monitoring the status of the job</returns>
        IJobInfo StartImport(string store, string fileName, string graphUri = Constants.DefaultGraphUri);

        /// <summary>
        /// Starts an export job
        /// </summary>
        /// <param name="store">The store to export data from</param>
        /// <param name="fileName">The name of the file in the brightstar\import folder to write to. This file will be overwritten if it already exists.</param>
        /// <param name="graphUri">The URI of the graph to be exported. If NULL, all graphs in the store are exported.</param>
        /// <returns>A JobInfo instance</returns>
        IJobInfo StartExport(string store, string fileName, string graphUri = null);

        /// <summary>
        /// Creates a new store file containing only the data required for the current state
        /// </summary>
        /// <param name="store">The store to consolidate</param>
        /// <returns>A JobInfo instance</returns>
        IJobInfo ConsolidateStore(string store);

        /// <summary>
        /// Returns the commit points of a Brightstar store
        /// </summary>
        /// <param name="storeName">The name of the store to examine</param>
        /// <param name="skip">How many commit points to skip over</param>
        /// <param name="take">How many commit points to return. Max allowed value is 100</param>
        /// <returns>An enumeration over the store commit points</returns>
        IEnumerable<ICommitPointInfo> GetCommitPoints(string storeName, int skip, int take);

        /// <summary>
        /// Returns the commit point that was in effect at a given date/time
        /// </summary>
        /// <param name="storeName">The name of the store to search</param>
        /// <param name="timestamp">The time to search for</param>
        /// <returns>An ICommitPointInfo representing the latest commit point in the store that was committed before the date/time specified by <paramref name="timestamp"/>. If
        /// there is no such commit point (either because the store was created after that date/time or the store has been coalesced and historical data removed),
        /// this method will return null.
        /// </returns>
        ICommitPointInfo GetCommitPoint(string storeName, DateTime timestamp);

        /// <summary>
        /// This will make the commit point provided the new latest commit point. Blocks until the operation is complete.
        /// </summary>
        /// <param name="storeName">The name of the store to be reverted</param>
        /// <param name="commitPoint">The commit point to revert to</param>
        void RevertToCommitPoint(string storeName, ICommitPointInfo commitPoint);

        /// <summary>
        /// Gets a list of transactions executed against this store
        /// </summary>
        /// <param name="storeName">Name of store for </param>
        /// <param name="skip">How many transactions to skip over</param>
        /// <param name="take">How many transactions to return.</param>
        /// <returns>An enumeration over the transactions executed against the store</returns>
        IEnumerable<ITransactionInfo> GetTransactions(string storeName, int skip, int take);

        /// <summary>
        /// Executes a previous transaction
        /// </summary>
        /// <param name="storeName">The name of the store to re-apply the transaction to</param>
        /// <param name="transactionInfo">The transaction to be applied</param>
        IJobInfo ReExecuteTransaction(string storeName, ITransactionInfo transactionInfo);

        /// <summary>
        /// Get a list of commit points that lie within a specified date/time range
        /// </summary>
        /// <param name="storeName">The name of the store to examine</param>
        /// <param name="latest">The latest commit date of commit points to be retrieved</param>
        /// <param name="earliest">The earliest commit date of the commit points to be retrieved</param>
        /// <param name="skip">The offset into the results list to return from</param>
        /// <param name="take">The number of results to return</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Raised if <paramref name="skip"/> is less than 0 or <paramref name="take"/> is greater than 100.</exception>
        IEnumerable<ICommitPointInfo> GetCommitPoints(string storeName, DateTime latest, DateTime earliest, int skip, int take);

        /// <summary>
        /// Retrieves the most recent statistics for the specified store
        /// </summary>
        /// <param name="storeName">The name of the store to retrieve statistics for.</param>
        /// <returns>A <see cref="IStoreStatistics"/> instance containing the most recent statistics for the named store, or NULL if
        /// there are no statistics availabe for the store.</returns>
        IStoreStatistics GetStatistics(string storeName);


        /// <summary>
        /// Retrieves a range of statistics records for a store
        /// </summary>
        /// <param name="storeName">The name of the store to retrieve statistics for</param>
        /// <param name="latest">The latest date to retrieve statistics for</param>
        /// <param name="earlierst">The earliest date to retrieve statisitcs for</param>
        /// <param name="skip">The offset into the date-filters list to return from</param>
        /// <param name="take">The number of results to return</param>
        /// <returns>An enumeration over the specified subset of statistics records for the store.</returns>
        /// <exception cref="ArgumentException">Raised if <paramref name="skip"/> is less than 0 or <paramref name="take"/> is greater than 100.</exception>
        IEnumerable<IStoreStatistics> GetStatistics(string storeName, DateTime latest, DateTime earlierst, int skip,
                                                    int take);

        /// <summary>
        /// Queues a job to update the statistics for a store
        /// </summary>
        /// <param name="storeName">The name of the store whose statistics are to be updated</param>
        /// <returns>A <see cref="IJobInfo"/> instance for tracking the current status of the job.</returns>
        IJobInfo UpdateStatistics(string storeName);

        /// <summary>
        /// Queues a job to create a snapshot of a store
        /// </summary>
        /// <param name="storeName">The name of the store to take a snapshot of</param>
        /// <param name="targetStoreName">The name of the store to be created to receive the snapshot</param>
        /// <param name="persistenceType">The type of persistence to use for the target store</param>
        /// <param name="sourceCommitPoint">OPTIONAL: the commit point in the source store to take a snapshot from</param>
        /// <returns>A <see cref="IJobInfo"/> instance for tracking the current status of the job.</returns>
        IJobInfo CreateSnapshot(string storeName, string targetStoreName, PersistenceType persistenceType, ICommitPointInfo sourceCommitPoint = null);
        
    }

}
