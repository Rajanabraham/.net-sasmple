package com.demo.repository;

import com.demo.model.FeatureImplementation;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface FeatureImplementationRepository extends JpaRepository<FeatureImplementation, Long> {
    List<FeatureImplementation> findByNameContainingIgnoreCase(String keyword);
}
